namespace UrlShortener.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }
        public string UserMessage { get; }

        public BusinessException(string errorCode, string userMessage, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            UserMessage = userMessage;
        }

        public BusinessException(string errorCode, string userMessage)
            : this(errorCode, userMessage, userMessage)
        {
        }
    }
}
