using Test.Domain.Entities;

namespace Test.Application.DTOs.RoomManagement
{
    public record TimeSlotDto(Guid Id, DateTime StartTime, DateTime EndTime, TimeSlotStatus Status);
}