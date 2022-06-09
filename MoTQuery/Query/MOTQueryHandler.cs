using MOTQuery.Interface;

namespace MOTQuery.Query
{
    internal class MOTQueryHandler : IQueryHandler
    {
        private readonly IValidator InputValidator;

        private readonly IConnector Connection;

        public MOTQueryHandler(IValidator validator, IConnector connection)
        {
            InputValidator = validator;
            Connection = connection;
        }

        public IOutput Process(IInput input)
        {
            if(InputValidator.IsValid(input))
            {
                IResponse response = Connection.Send(input);

                if (response.Success)
                {
                    //TODO: Add Response validation
                    return new MOTOutput(response);
                }
                else
                {
                    return new ErrorOutput(response.ErrorMessage);
                }
            } 
            else
            {
                return new ErrorOutput(InputValidator.ErrorMessage);
            }
        }
    }
}
