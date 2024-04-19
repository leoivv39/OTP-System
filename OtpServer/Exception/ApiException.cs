namespace OtpServer.Exception
{
    public class ApiException : System.Exception
    {
        public ApiException(string message, string errorCode, int statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public string ErrorCode { get; }
        public int StatusCode { get; }
    }
}
