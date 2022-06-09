using MOTQuery.Interface;

namespace MOTQuery.Query
{
    internal class ErrorOutput : IOutput
    {
        private readonly string ErrorMessage;

        public ErrorOutput(string errorMessage)
        {
            ErrorMessage = errorMessage ?? "";
        }

        public string Display()
        {
            return ErrorMessage;
        }
    }
}
