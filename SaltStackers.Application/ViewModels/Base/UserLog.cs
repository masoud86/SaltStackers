using SaltStackers.Application.ViewModels.Log;
using System.Text.Json.Serialization;

namespace SaltStackers.Application.ViewModels.Base
{
    public class UserLog
    {
        [JsonIgnore]
        public string? LogUserId { get; set; }

        [JsonIgnore]
        public ClientInformation? LogInfo { get; set; }
    }
}
