using System.Text.Json.Serialization;

namespace CFS.Models
{
    public class CspReportRequest
    {
        [JsonPropertyName("csp-report")]
        public CspReport CspReport { get; set; }
    }
}
