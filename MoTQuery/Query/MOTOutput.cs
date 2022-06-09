using MOTQuery.Interface;
using System.Text;
using System.Text.Json.Nodes;

namespace MOTQuery.Query
{
    internal class MOTOutput : IOutput
    {
        public string Make { get; internal set; }
        public string Model { get; internal set; }
        public string Colour { get; internal set; }
        public DateTime ExpiryDate { get; internal set; }
        public int NumMotFailures { get; internal set; }

        public MOTOutput(IResponse response)
        {
            if (response != null && !String.IsNullOrEmpty(response.Body))
            {
                ParseResponse(response);
            }
        }

        private void ParseResponse(IResponse response)
        {
            JsonNode node = JsonNode.Parse(response.Body)![0]!;

            Make = node["make"]?.ToJsonString().Replace("\"", "") ?? "Make not Available";
            Model = node["model"]?.ToJsonString().Replace("\"", "") ?? "Model not Available";
            Colour = node["primaryColour"]?.ToJsonString().Replace("\"", "") ?? "Colour not Available";

            if (node!["motTests"] == null)
            {
                ExpiryDate = DateTime.MinValue;
                NumMotFailures = 0;
            }
            else
            {
                JsonArray motNodes = node!["motTests"]!.AsArray();

                if (motNodes.Count != 0)
                {
                    ExpiryDate = DateTime.Parse(motNodes[0]!["expiryDate"]?.ToJsonString().Replace("\"", "") ?? "1900-01-01");
                }

                foreach (JsonNode test in motNodes)
                {
                    if (string.Equals(test!["testResult"]?.ToJsonString().Replace("\"", "").ToUpper() ?? "", "FAILED"))
                    {
                        NumMotFailures++;
                    }
                }

            }
        }

        public string Display()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Make: {Make}");
            builder.AppendLine($"Model: {Model}");
            builder.AppendLine($"Colour: {Colour}");
            builder.AppendLine($"Expiry Date: {ExpiryDate.ToShortDateString()}");
            builder.AppendLine($"Number of previous MoT failures: {NumMotFailures}");

            return builder.ToString();
        }
    }
}
