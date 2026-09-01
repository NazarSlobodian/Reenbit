using System.Net.Http.Json;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;

using Test.Application.DTOs.Auth;
using Test.Domain.Entities;
using Test.Infrastructure.Persistence;

namespace Test.IntegrationTests
{
    public class BookingConcurrencyTests : IClassFixture<BookingApiFactory>
    {
        private readonly BookingApiFactory _factory;

        public BookingConcurrencyTests(BookingApiFactory factory) => _factory = factory;

        [Fact]
        public async Task Concurrent_Booking_Requests_For_Same_Slot_Only_One_Succeeds()
        {
            var ct = TestContext.Current.CancellationToken;

            // Arrange: create a room (which generates its slot grid) and grab one real slot ID directly from the DB.
            using var setupScope = _factory.Services.CreateScope();
            var context = setupScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var room = new Room { Name = "Concurrency Test Room" };
            context.Rooms.Add(room);
            var slot = new TimeSlot
            {
                RoomId = room.Id,
                Room = room,
                StartTime = DateTime.UtcNow.Date.AddDays(1).AddHours(10),
                EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(11),
                Status = TimeSlotStatus.Available
            };
            context.TimeSlots.Add(slot);
            await context.SaveChangesAsync(ct);

            const int concurrentRequests = 15;
            var userTokens = new List<string>();

            using var authClient = _factory.CreateClient();
            for (int i = 0; i < concurrentRequests; i++)
            {
                var register = await authClient.PostAsJsonAsync("/api/auth/register",
                    new RegisterDto($"user{i}@example.com", "Password123!", $"User {i}"), ct);

                register.EnsureSuccessStatusCode();

                var auth = await register.Content.ReadFromJsonAsync<AuthResultDto>(cancellationToken: ct);
                userTokens.Add(auth!.Token);
            }

            // Act: fire N concurrent booking requests at the same slot.
            var tasks = userTokens.Select(token =>
            {
                var client = _factory.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                return client.PostAsync($"/api/bookings/{slot.Id}", content: null, ct);
            });

            var responses = await Task.WhenAll(tasks);

            // Assert: exactly one success, everything else a clean conflict — never a 500, never silence.
            responses.Count(r => r.StatusCode == HttpStatusCode.OK).Should().Be(1);
            responses.Count(r => r.StatusCode == HttpStatusCode.Conflict).Should().Be(concurrentRequests - 1);
            responses.Should().NotContain(r => (int)r.StatusCode >= 500);

            // Confirm the database itself agrees — exactly one Booking row for this slot, not just one 200 response.
            using var verifyScope = _factory.Services.CreateScope();
            var verifyContext = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var bookingCount = await verifyContext.Bookings.CountAsync(b => b.TimeSlotId == slot.Id, ct);
            bookingCount.Should().Be(1);
        }
    }
}