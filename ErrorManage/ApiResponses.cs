namespace MicroFinance.ErrorManage
{
    public class ApiResponses
    {
        public ApiResponses(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                0 => "Password Pattern Doesn't match",
                1 => "Name Pattern Doesn't Match",
                400 => "A bad request",
                401 => "UnAuthorized",
                404 => "Resource not found",
                500 => "Errors are the path to the dark side.  Errors lead to anger.   Anger leads to hate.  Hate leads to career change.",
                _ => null
            };
        }
    }
}