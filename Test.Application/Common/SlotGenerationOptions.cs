namespace Test.Application.Common
{
    public class SlotGenerationOptions
    {
        public int SlotDurationMinutes { get; set; } = 60;
        public TimeSpan WorkingHoursStart { get; set; } = new(9, 0, 0);
        public TimeSpan WorkingHoursEnd { get; set; } = new(18, 0, 0);
        public int HorizonDays { get; set; } = 14;
    }
}