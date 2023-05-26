namespace MicroFinance.Exceptions
{
    public class BadRequestExceptionHandler : Exception
    {
        public BadRequestExceptionHandler(string msg) : base(msg)
        {}
    }
}