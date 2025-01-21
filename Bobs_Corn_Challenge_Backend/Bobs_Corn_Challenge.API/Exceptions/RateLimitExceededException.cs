namespace Bobs_Corn_Challenge.API.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException(string message) : base(message) { }
    }
}