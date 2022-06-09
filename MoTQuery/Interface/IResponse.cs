namespace MOTQuery.Interface
{
    internal interface IResponse
    {
        bool Success { get; }
        string ErrorMessage { get; }
        string Body { get; }
    }
}
