using Moq.Protected;
using System.Net;
using System.Collections;

namespace MOTQueryTests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(typeof(HttpConnectionTestCases))]
        public void TestHttpConnectorSend(string regNumber, bool expectedSuccess)
        {
            var mockMessageHandler = TestUtils.GetMockHttpMessageHandler();

            var connector = new MOTQuery.Connectors.HttpConnector("http://localhost", "", mockMessageHandler.Object);

            var input = new MOTQuery.Query.MOTInput(regNumber);

            var output = connector.Send(input);

            //Ensure correct outcome based on reg number found
            Assert.That(output.Success, Is.EqualTo(expectedSuccess));

            //Ensure if expected failure then error message present
            if(!expectedSuccess)
            {
                Assert.IsTrue(!String.IsNullOrEmpty(output.ErrorMessage));
            }
        }

        [TestCaseSource(typeof(MOTInputTestCases))]
        public void TestMOTInput(string regNum, string expected)
        {
            MOTQuery.Query.MOTInput input = new MOTQuery.Query.MOTInput(regNum);

            Assert.That(input.RegistrationNumber, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(ErrorOutputTestCases))]
        public void TestErrorOutput(string errorMsg, string expected)
        {
            MOTQuery.Query.ErrorOutput output = new MOTQuery.Query.ErrorOutput(errorMsg);

            Assert.That(output.Display(), Is.EqualTo(expected));
        }


        [TestCaseSource(typeof(MOTOutputTestCases))]
        public void TestMOTOuput(string responseBody, string expected)
        {
            MOTQuery.Connectors.HttpResponse response = new(){
                Success = true,
                Body = responseBody
            };

            MOTQuery.Query.MOTOutput output = new MOTQuery.Query.MOTOutput(response);

            Assert.That(output.Display(), Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(UKStandardPlateValidatorTestCases))]
        public void TestUKStandardPlateValidator(string regNum, bool expectedOutcome, string expectedErrorMessage)
        {
            MOTQuery.Query.MOTInput input = new MOTQuery.Query.MOTInput(regNum);
            MOTQuery.Validators.UKStandardPlateValidator validator = new();

            Assert.That(validator.IsValid(input), Is.EqualTo(expectedOutcome));
            Assert.That(validator.ErrorMessage, Is.EqualTo(expectedErrorMessage));
        }

        class HttpConnectionTestCases : IEnumerable
        { 
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "TEST123", true };
                yield return new object[] { "BADREG1", false };
                yield return new object[] { "", false };
                yield return new object[] { null, false };
            }
        }

        class MOTInputTestCases : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "TEST123", "TEST123" };
                yield return new object[] { "    TEST123", "TEST123" };
                yield return new object[] { "    T ES   T 1  2  3   ", "TEST123" };
                yield return new object[] { "", "" };
                yield return new object[] { "      ", "" };
                yield return new object[] { null, "" };
            }
        }

        class ErrorOutputTestCases : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "TEST123", "TEST123" };
                yield return new object[] { "", "" };
                yield return new object[] { null, "" };
            }
        }

        //TODO: Change horrible strings to JSON serializable test class
        class MOTOutputTestCases : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "[ { \"registration\": \"WJ03XUS\", \"make\": \"HONDA\", \"model\": \"CR-V\", \"firstUsedDate\": \"2003.03.04\", \"fuelType\": \"Petrol\", \"primaryColour\": \"Silver\", \"motTests\": [ { \"completedDate\": \"2021.09.13 15:35:08\", \"testResult\": \"PASSED\" }, { \"completedDate\": \"2021.09.13 11:08:51\", \"testResult\": \"FAILED\" }, { \"completedDate\": \"2020.09.09 08:25:48\", \"testResult\": \"PASSED\" }, { \"completedDate\": \"2019.11.25 13:11:07\", \"testResult\": \"PASSED\" } ] } ]", $"Make: HONDA{Environment.NewLine}Model: CR-V{Environment.NewLine}Colour: Silver{Environment.NewLine}Expiry Date: 01/01/1900{Environment.NewLine}Number of previous MoT failures: 1{Environment.NewLine}" };
                yield return new object[] { "", $"Make: {Environment.NewLine}Model: {Environment.NewLine}Colour: {Environment.NewLine}Expiry Date: 01/01/0001{Environment.NewLine}Number of previous MoT failures: 0{Environment.NewLine}" };
                yield return new object[] { null, $"Make: {Environment.NewLine}Model: {Environment.NewLine}Colour: {Environment.NewLine}Expiry Date: 01/01/0001{Environment.NewLine}Number of previous MoT failures: 0{Environment.NewLine}" };
            }
        }

        class UKStandardPlateValidatorTestCases : IEnumerable
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return new object[] { "TEST123", true, "" };
                yield return new object[] { "  TE S  T 1 2 3  ", true, "" };
                yield return new object[] { "TEST12", false, "The entered registration number is the incorrect length" };
                yield return new object[] { "TEST1234", false, "The entered registration number is the incorrect length" };
                yield return new object[] { "", false, "You must enter a registration number" };
            }
        }
    }
}