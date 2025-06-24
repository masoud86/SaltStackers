namespace SaltStackers.Application.ViewModels.Log
{
    public class ClientInformation
    {
        public string Device { get; set; }

        public string IpAddress { get; set; }

        public string Browser { get; set; }

        public string BrowserVersion { get; set; }

        public string OperatingSystem { get; set; }

        public string BrowserFull => $"{Browser} {BrowserVersion}";

        public string DeviceFull => $"{Device} {OperatingSystem}";

        public string UserId { get; set; }
    }
}
