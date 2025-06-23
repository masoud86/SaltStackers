using SaltStackers.Application.ViewModels.Customer;

namespace SaltStackers.Application.ViewModels.Api
{
    public class RegisterResponseApi : JwtToken
    {
        public bool Succeeded { get; set; }

        public string? ErrorMessage { get; set; }

        public CustomerInformation? CustomerInformation { get; set; }
    }
}
