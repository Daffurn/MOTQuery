using Moq.Protected;
using System.Net;
using System.Collections;
using MOTQuery.Interface;
using MOTQuery.Query;

namespace MOTQueryTests
{
    public class IntegrationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(typeof(ApplicationRunTestCases))]
        public void TestApplicationRun(string regNum, string body, string expectedOutput)
        {
            MockReader reader = new(regNum);
            MockWriter writer = new();
            MOTQuery.Validators.UKStandardPlateValidator validator = new();

            var mockMessageHandler = TestUtils.GetMockHttpMessageHandler(body);
            var connector = new MOTQuery.Connectors.HttpConnector("http://localhost", "", mockMessageHandler.Object);

            MOTQueryHandler queryHandler = new MOTQueryHandler(validator, connector);

            writer.Write(queryHandler.Process(reader.GetInput()));

            Assert.That(writer.Output, Is.EqualTo(expectedOutput));
        }

        class MockReader : IInputReader
        {
            private readonly string RegNum;

            public MockReader(string regNum)
            {
                RegNum = regNum;
            }

            public IInput GetInput()
            {
                return new MOTInput(RegNum);
            }
        }

        class MockWriter : IOutputWriter
        {
            public string Output { get; private set;}

            public MockWriter()
            {
                Output = "";
            }

            public void Write(IOutput output)
            {
                Output = output.Display();
            }
        }

        //TODO: Change horrible strings to JSON serializable test class
        class ApplicationRunTestCases : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "BADREG1", "", @"One or more errors occurred. (Response status code does not indicate success: 404 (Not Found).)" };
                yield return new object[] { "TEST123", "[ { \"registration\": \"WJ03XUS\", \"make\": \"HONDA\", \"model\": \"CR-V\", \"firstUsedDate\": \"2003.03.04\", \"fuelType\": \"Petrol\", \"primaryColour\": \"Silver\", \"motTests\": [ { \"completedDate\": \"2021.09.13 15:35:08\", \"testResult\": \"PASSED\" }, { \"completedDate\": \"2021.09.13 11:08:51\", \"testResult\": \"FAILED\" }, { \"completedDate\": \"2020.09.09 08:25:48\", \"testResult\": \"PASSED\" }, { \"completedDate\": \"2019.11.25 13:11:07\", \"testResult\": \"PASSED\" } ] } ]", $"Make: HONDA{Environment.NewLine}Model: CR-V{Environment.NewLine}Colour: Silver{Environment.NewLine}Expiry Date: 01/01/1900{Environment.NewLine}Number of previous MoT failures: 1{Environment.NewLine}" };
                yield return new object[] { "", "", "You must enter a registration number" };
                yield return new object[] { "TEST12", "", "The entered registration number is the incorrect length" };
                yield return new object[] { "TEST1234", "", "The entered registration number is the incorrect length" };
            }
        }
    }
}