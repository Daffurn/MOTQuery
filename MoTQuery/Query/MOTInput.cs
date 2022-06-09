using MOTQuery.Interface;

namespace MOTQuery.Query
{
    internal class MOTInput : IInput
    {
        private string _registrationNumber;
        public string RegistrationNumber { get => _registrationNumber; set => _registrationNumber = value; }

        public MOTInput(string regNum)
        {
            _registrationNumber = new string(regNum?.Where(c => !Char.IsWhiteSpace(c))
                                                    .ToArray()) ?? "";
        }
    }
}
