using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Membership.User;
using SaltStackers.Domain.Models.Membership;
using Microsoft.AspNetCore.Identity;

namespace SaltStackers.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IdentityResult> CreateCustomerAsync(CustomerDto model);

        Task<List<CustomerDto>> GetCustomersAsync(CustomerFilters filter);

        Task<Customers> GetCustomersModelAsync(CustomerFilters filter);

        Task<CustomerDto> GetCustomerAsync(string id);

        Task<IdentityResult> UpdateCustomerAsync(CustomerDto model);
        
        Task<CustomerProfileApi> GetCustomerProfileByUsernameAsync(string username);

        Task<ServiceResult> UpdateCustomerProfileAsync(CustomerProfileApi model, string username);

        Task<int> CountCustomersAsync(CustomerFilters filter);

        Task<AspNetUser> FindUserByPhoneNumber(string phoneNumber);

        Task<RegisterResult> RegisterCustomerAsync(RegisterCustomer model); 
        
        Task<CustomerInformation?> GetCustomerInformationAsync(string username);

        Task<ServiceResult> EditUsernameAsync(EditUsername model);

        Task<ServiceResult> EditAboutAsync(EditAbout model);
    }
}
