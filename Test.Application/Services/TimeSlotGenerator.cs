using Test.Application.Common;
using Test.Application.Interfaces.Services;
using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class TimeSlotGenerator : ITimeSlotGenerator
    {
        public IEnumerable<TimeSlot> GenerateForRoom(Guid roomId, DateTime fromDateUtc, SlotGenerationOptions options)
        {
            var timeSlots = new List<TimeSlot>();

            for (var day = 0; day < options.HorizonDays; day++)
            {
                var date = fromDateUtc.Date.AddDays(day);
                var cursor = date + options.WorkingHoursStart;
                var dayEnd = date + options.WorkingHoursEnd;

                while (cursor.AddMinutes(options.SlotDurationMinutes) <= dayEnd)
                {
                    var slotEnd = cursor.AddMinutes(options.SlotDurationMinutes);
                    timeSlots.Add(new TimeSlot
                    {
                        RoomId = roomId,
                        StartTime = cursor,
                        EndTime = slotEnd,
                        Status = TimeSlotStatus.Available
                    });
                    cursor = slotEnd;
                }
            }

            return timeSlots;
        }
    }
}