using MOTQuery.Interface;

namespace MOTQuery.Validators
{
    internal class UKStandardPlateValidator : IValidator
    {
        public string ErrorMessage { get; private set; }

        public UKStandardPlateValidator()
        {
            ErrorMessage = "";
        }

        public bool IsValid(IInput input)
        {
            if(String.IsNullOrEmpty(input.RegistrationNumber))
            {
                ErrorMessage = "You must enter a registration number";
                return false;
            }

            if (input.RegistrationNumber.Length != 7)
            {
                ErrorMessage = "The entered registration number is the incorrect length";
                return false;
            }

            return true;
        }
    }
}
