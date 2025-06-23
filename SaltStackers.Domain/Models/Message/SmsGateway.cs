namespace SaltStackers.Domain.Models.Message
{
    public class SmsGateway
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Company { get; set; }
    }
}
