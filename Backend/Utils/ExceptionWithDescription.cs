namespace Backend.Utils
{
    public class ExceptionWithDescription : Exception
    {
        public ExceptionWithDescription(string text):base(text)
        {

        }

    }
    public class UserNotAuthorized : Exception
    {
        public UserNotAuthorized(string text) : base(text)
        {

        }

    }
}
