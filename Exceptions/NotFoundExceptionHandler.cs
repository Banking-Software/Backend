namespace MicroFinance.Exceptions   
{
    public class NotFoundExceptionHandler : Exception
    {
        public NotFoundExceptionHandler(string msg) : base(msg)
        { }
    }
}