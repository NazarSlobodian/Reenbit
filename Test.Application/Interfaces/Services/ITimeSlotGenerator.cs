using Test.Application.Common;
using Test.Domain.Entities;

namespace Test.Application.Interfaces.Services
{
    public interface ITimeSlotGenerator
    {
        IEnumerable<TimeSlot> GenerateForRoom(Guid roomId, DateTime fromDateUtc, SlotGenerationOptions options);
    }
}