namespace MOTQuery.Interface
{
    internal interface IValidator
    {
        public string ErrorMessage { get; }

        public bool IsValid(IInput input);
    }
}
