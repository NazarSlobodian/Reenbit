namespace Test.Application.Exceptions
{
    public class BookingConflictException : Exception
    {
        public BookingConflictException(string message) : base(message) { }
    }
}