using MOTQuery.Readers;
using MOTQuery.Query;
using MOTQuery.Validators;
using MOTQuery.Connectors;
using MOTQuery.Writers;

ConsoleReader consoleReader = new();
ConsoleWriter consoleWriter = new();
UKStandardPlateValidator validator = new ();

//TODO: Change this to take an IConfiguration and implement configuration parsing from appsettings.json / Command Line arguments
HttpConnector connector = new HttpConnector("https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests", "HybH0yr4Hj3eEgybT9pkn6B7PA769YDa8kt4wKdp");

MOTQueryHandler queryHandler = new MOTQueryHandler(validator, connector);

while (true)
{
    consoleWriter.Write(queryHandler.Process(consoleReader.GetInput()));
}
