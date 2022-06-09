namespace MOTQuery.Interface
{
    internal interface IConnector
    {
        IResponse Send(IInput input);
    }
}
