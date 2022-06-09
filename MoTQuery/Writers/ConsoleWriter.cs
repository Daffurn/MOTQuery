using MOTQuery.Interface;

namespace MOTQuery.Writers
{
    internal class ConsoleWriter : IOutputWriter
    {
        public void Write(IOutput output)
        {
            Console.WriteLine(output.Display());
        }
    }
}
