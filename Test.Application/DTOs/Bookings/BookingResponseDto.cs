namespace Test.Application.DTOs.Bookings
{
    public record BookingResponseDto(Guid Id, decimal RoomPriceTotal, decimal ServicesPriceTotal, decimal TotalPrice);
}
