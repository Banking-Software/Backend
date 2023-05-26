namespace MicroFinance.Exceptions
{
    public class KeyNotFoundExceptionHandler : Exception
    {
        public KeyNotFoundExceptionHandler(string msg) : base(msg)
        {}
    }
}