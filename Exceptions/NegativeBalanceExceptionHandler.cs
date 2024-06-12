namespace MicroFinance.Exceptions
{
    public class NegativeBalanceExceptionHandler : Exception
    {
        public NegativeBalanceExceptionHandler(string msg) : base(msg)
        {}
    }
}