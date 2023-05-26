namespace MicroFinance.Exceptions
{
    public class UnAuthorizedExceptionHandler : Exception
    {
        public UnAuthorizedExceptionHandler(string msg) : base(msg)
        { }
    }
}