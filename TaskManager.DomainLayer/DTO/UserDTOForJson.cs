
using System.Text.Json.Serialization;

namespace TaskManager.DomainLayer.DTO
{
    public class UserDTOForJson
    {
        [JsonPropertyName("job")]
        public string Job { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("techLeaderLogin")]
        public string? TechLeaderLogin { get; set; }
    }
}
