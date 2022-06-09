using MOTQuery.Interface;
using MOTQuery.Query;

namespace MOTQuery.Readers
{
    internal class ConsoleReader : IInputReader
    {
        public IInput GetInput()
        {
            Console.WriteLine("Enter a valid UK car registration:");

            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                return new MOTInput(input);
            }

            return new MOTInput("");
        }
    }
}
