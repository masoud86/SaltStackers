namespace SaltStackers.Application.ViewModels.Message
{
    public class MessageSendStatus
    {
        public bool Succeeded { get; set; }

        public string Error { get; set; }

        public string ErrorDescription { get; set; }

        public string TrackingCode { get; set; }
    }
}
