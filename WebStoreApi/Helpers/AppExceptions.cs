namespace WebStoreApi.Helpers
{
    public class AppExceptions : Exception
    {

        public AppExceptions() : base() { }
        public AppExceptions(string message) : base(message) { }
        public AppExceptions(string message, Exception innerException) : base(message, innerException) { }


        public override string Message => $"Username or password is invalid ";
    }
}
