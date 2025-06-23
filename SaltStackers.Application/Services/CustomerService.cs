using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SaltStackers.Application.Filters;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Membership.User;
using SaltStackers.Common.Helper;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMembershipService _membershipService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogService _logService;
        private readonly IMapper _iMapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IFinancialRepository _financialRepository;
        private readonly IConfiguration _configuration;

        public CustomerService(IMembershipService membershipService, ICustomerRepository customerRepository,
            ILogService logService, IEmailService emailService, UserManager<AspNetUser> userManager,
            SignInManager<AspNetUser> signInManager, IFinancialRepository financialRepository,
            IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _iMapper = config.CreateMapper();
            _membershipService = membershipService;
            _customerRepository = customerRepository;
            _logService = logService;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _financialRepository = financialRepository;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateCustomerAsync(CustomerDto model)
        {
            var user = _iMapper.Map<UserDto>(model);
            var createUser = await _membershipService.CreateUserAsync(user);
            if (createUser.Item1.Succeeded)
            {
                var assignRole = await _membershipService.AddUserToRoleAsync(user.Username, "Customer");
            }
            return IdentityResult.Failed(new IdentityError { Description = Resources.Error.DatabaseInsert });

        }

        public async Task<List<CustomerDto>> GetCustomersAsync(CustomerFilters filter)
        {
            var predicate = CustomerFilter.ToExpression(filter);

            var model = await _customerRepository.GetCustomersAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<AspNetUser>, List<CustomerDto>>(model);
        }

        public async Task<Customers> GetCustomersModelAsync(CustomerFilters filter)
        {
            var predicate = CustomerFilter.ToExpression(filter);

            var recordTotal = await _customerRepository.GetCustomersCountAsync();

            var recordsFilters = await _customerRepository.GetCustomersCountAsync(predicate);

            return new Customers
            {
                Items = await GetCustomersAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<CustomerDto> GetCustomerAsync(string id)
        {
            var user = await _membershipService.FindUserByIdAsync(id);
            var customer = _iMapper.Map<CustomerDto>(user);
            return customer;
        }

        public async Task<IdentityResult> UpdateCustomerAsync(CustomerDto model)
        {
            return await _membershipService.UpdateUserAsync(_iMapper.Map<EditUser>(model));
        }

        public async Task<CustomerProfileApi> GetCustomerProfileByUsernameAsync(string username)
        {
            var user = await _membershipService.FindUserByNameAsync(username);
            var customerProfile = _iMapper.Map<CustomerProfileApi>(user);
            if (!string.IsNullOrEmpty(user.Referral))
            {
                var referral = await _membershipService.FindUserByNameAsync(user.Referral);
                customerProfile.Referral = referral.Name;
            }
            return customerProfile;
        }

        public async Task<ServiceResult> UpdateCustomerProfileAsync(CustomerProfileApi model, string username)
        {
            var user = await _membershipService.FindUserByNameAsync(username);

            var updateUser = new EditUser
            {
                Id = user.Id,
                Name = model.Name,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                BloodType = model.BloodType
            };

            if (string.IsNullOrEmpty(user.Referral) && !string.IsNullOrEmpty(model.Referral) &&
                username.ToLower() != model.Referral.ToLower())
            {
                var referral = await _membershipService.FindUserByNameAsync(model.Referral);
                if (referral == null)
                {
                    return new ServiceResult(false, new()
                    {
                        new() { Level = ErrorLevel.Blocker, Description = "Referral code not exists!" }
                    });
                }
                updateUser.Referral = referral.UserName;
            }

            await _membershipService.UpdateUserAsync(updateUser);

            return new ServiceResult(true);
        }

        public async Task<int> CountCustomersAsync(CustomerFilters filter)
        {
            var predicate = CustomerFilter.ToExpression(filter);
            return await _customerRepository.GetCustomersCountAsync(predicate);
        }

        public async Task<AspNetUser> FindUserByPhoneNumber(string phoneNumber)
        {
            return await _customerRepository.FindUserByPhoneNumber(phoneNumber);
        }

        public async Task<RegisterResult> RegisterCustomerAsync(RegisterCustomer model)
        {
            var currentUser = await _customerRepository.FindCustomerByEmailAsync(model.Email.Trim().ToUpper());
            if (currentUser != null)
            {
                return new RegisterResult { Succeeded = false, ErrorMessage = "Already exists!" };
            }

            if (!string.IsNullOrEmpty(model.Referral))
            {
                var referral = await _userManager.FindByNameAsync(model.Referral);
                if (referral == null)
                {
                    return new RegisterResult { Succeeded = false, ErrorMessage = "Wrong referral code" };
                }
            }

            var email = model.Email.Trim().ToLower();
            var userName = UserHelper.UsernameGenerator(model.Name, model.Email);
            var hasher = new PasswordHasher<IdentityUser>();
            var user = new AspNetUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                PhoneNumber = model.PhoneNumber,
                PasswordHash = hasher.HashPassword(null, model.Password),
                Referral = string.IsNullOrEmpty(model.Referral) ? null : model.Referral,
                LastLogin = DateTime.UtcNow
            };

            var register = await _customerRepository.CreateCustomerAsync(user);

            if (register)
            {
                await _emailService.Register(user.Id);

                return new RegisterResult { Succeeded = true, Username = userName };
            }

            return new RegisterResult { Succeeded = false, ErrorMessage = "Unkown Error" };
        }

        public async Task<CustomerInformation?> GetCustomerInformationAsync(string username)
        {
            var user = await _membershipService.FindUserByNameAsync(username);
            if (user == null)
            {
                return null;
            }

            return _iMapper.Map<CustomerInformation>(user);
        }

        public async Task<ServiceResult> EditUsernameAsync(EditUsername model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ServiceResult(false);
            }
            user.UserName = model.Username;
            user.NormalizedUserName = model.Username.ToUpper();
            await _userManager.UpdateAsync(user);
            return new ServiceResult(true);
        }

        public async Task<ServiceResult> EditAboutAsync(EditAbout model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ServiceResult(false);
            }
            await _userManager.UpdateAsync(user);
            return new ServiceResult(true);
        }
    }
}
