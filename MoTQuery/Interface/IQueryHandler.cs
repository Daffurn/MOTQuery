namespace MOTQuery.Interface
{
    internal interface IQueryHandler
    {
        public IOutput Process(IInput request);
    }
}
