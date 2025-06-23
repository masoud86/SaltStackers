using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Membership.User;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;
using System.Security.Claims;

namespace SaltStackers.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly ILoggerService<MembershipService> _loggerService;
        private readonly ILogService _logService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _iMapper;

        public MembershipService(UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager,
            RoleManager<AspNetRole> roleManager, ILoggerService<MembershipService> loggerService,
            ILogService logService, ICustomerRepository customerRepository)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.CreateMap<AspNetUser, UserDto>()
                    .ForMember(dest => dest.Username, map => map.MapFrom(source => source.UserName))
                    .ForMember(dest => dest.Role, map => map.MapFrom(source => GetRoleByUserIdAsync(source.Id).Result))
                    .ForMember(dest => dest.RoleModel, map => map.MapFrom(source => GetRoleModelByUserIdAsync(source.Id).Result));
                cfg.CreateMap<AspNetUser, EditUser>()
                    .ForMember(dest => dest.Role, map => map.MapFrom(source => GetRoleByUserIdAsync(source.Id).Result));
                cfg.CreateMap<UserDto, AspNetUser>()
                    .ForMember(dest => dest.UserName, map => map.MapFrom(source => source.Username));
            });
            _iMapper = config.CreateMapper();

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _loggerService = loggerService;
            _logService = logService;
            _customerRepository = customerRepository;
        }

        public async Task<SignInResult> LoginAsync(Login model)
        {
            var login = await _signInManager
                .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            if (login.Succeeded)
            {
                var user = await FindUserByNameAsync(model.Username);
                model.Password = "*******";
                await _logService.AddUserLogAsync(user.Id, "User", "LoggedIn", "", model, model.LogInfo);
            }
            return login;
        }

        public async Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public async Task<AspNetUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AspNetUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AspNetUser> FindUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUser model)
        {
            var user = _iMapper.Map<AspNetUser>(model);
            user.IsAdmin = true;
            var createUser = await _userManager.CreateAsync(user, model.Password);
            if (createUser.Succeeded)
            {
                var fullName = model.Name;
                await _logService.AddUserLogAsync(model.LogUserId, "User", "CreateUser", fullName, model, model.LogInfo);
            }
            return createUser;
        }

        public async Task<Tuple<IdentityResult, string>> CreateUserAsync(UserDto model)
        {
            var user = _iMapper.Map<AspNetUser>(model);
            user.Id = Guid.NewGuid().ToString();
            var createUser = await _userManager.CreateAsync(user);
            if (createUser.Succeeded)
            {
                var fullName = model.Name;
                await _logService.AddUserLogAsync(model.LogUserId, "User", "CreateUser", fullName, model, model.LogInfo);
            }
            return Tuple.Create(createUser, createUser.Succeeded ? user.Id : "");
        }

        public async Task<EditUser> GetUserForEditAsync(string userId)
        {
            var user = await FindUserByIdAsync(userId);
            return _iMapper.Map<EditUser>(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(EditUser model)
        {
            var user = await FindUserByIdAsync(model.Id);
            user.Name = model.Name;

            if (user.PhoneNumber != model.PhoneNumber.Trim())
            {
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }

            if (user.Email != model.Email.Trim())
            {
                user.UserName = model.Email;
                user.Email = model.Email;
                user.EmailConfirmed = false;
            }

            if (!string.IsNullOrEmpty(model.Referral))
            {
                user.Referral = model.Referral;
            }

            user.EditDateTime = DateTime.UtcNow;
            var updateUser = await _userManager.UpdateAsync(user);
            if (updateUser.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.LogUserId) && model.LogInfo != null)
                {
                    var fullName = user.Name;
                    await _logService.AddUserLogAsync(model.LogUserId, "User", "EditUser", fullName, model, model.LogInfo);
                }
            }
            return updateUser;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AspNetUser model, string token)
        {
            return await _userManager.ConfirmEmailAsync(model, token);
        }

        public async Task<IdentityResult> ConfirmPhoneNumberAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            user.PhoneNumberConfirmed = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> IsEmailFree(string email)
        {
            return await FindUserByEmailAsync(email) == null;
        }

        public async Task<bool> IsUsernameFree(string username)
        {
            return await FindUserByNameAsync(username) == null;
        }

        public async Task<string> GenerateEmailConfirmationLinkAsync(CreateUser model, string url)
        {
            var user = _iMapper.Map<AspNetUser>(model);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return $"{url}?username={model.Email}&token={token}";
        }

        public bool IsSignedIn(ClaimsPrincipal model)
        {
            return _signInManager.IsSignedIn(model);
        }

        public async Task<bool> IsBlockedAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return true;
            }

            var user = await FindUserByIdAsync(userId);
            if (user == null)
            {
                return true;
            }
            return user.IsBlocked;
        }

        public async Task CreateThirdPartyUserAsync(string email, ExternalLoginInfo externalLoginInfo)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
            {
                var userName = email.Split('@')[0];
                user = new AspNetUser
                {
                    UserName = (userName.Length <= 20 ? userName : userName.Substring(0, 20)),
                    Email = email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);
            }

            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManager.SignInAsync(user, false);
        }

        public void SignOut()
        {
            _signInManager.SignOutAsync();
        }

        public async Task<Users> GetUsersAsync(UserFilters filter)
        {
            var model = _userManager.Users;
            var recordTotal = await model.CountAsync();

            if (!string.IsNullOrEmpty(filter.Query))
            {
                model = model.Where(p =>
                    p.Name.ToLower().Contains(filter.Query) ||
                    p.UserName.ToLower().Contains(filter.Query) ||
                    p.Email.ToLower().Contains(filter.Query)
                );
            }

            var recordsFilters = model.Count();

            switch (filter.Sort.ToLower())
            {
                case "name":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.Name) : model.OrderByDescending(p => p.Name);
                    break;
                case "username":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.UserName) : model.OrderByDescending(p => p.UserName);
                    break;
                case "createtime":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.CreateDateTime) : model.OrderByDescending(p => p.CreateDateTime);
                    break;
                default:
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.EditDateTime) : model.OrderByDescending(p => p.EditDateTime);
                    break;
            }

            model = model.Skip(filter.Start).Take(filter.PageSize);

            var users = _iMapper.Map<List<UserDto>>(await model.AsNoTracking().ToListAsync());

            users.ForEach(p => p.RoleModel = _iMapper.Map<RoleDto>(GetRoleModelByUserIdAsync(p.Id).Result));

            return new Users
            {
                Items = users,
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<Roles> GetRolesAsync(RoleFilters filter)
        {
            var model = _roleManager.Roles;
            var recordTotal = model.Count();
            if (!string.IsNullOrEmpty(filter.Query))
            {
                model = model.Where(p =>
                    p.DisplayName.ToLower().Contains(filter.Query) ||
                    p.Name.ToLower().Contains(filter.Query) ||
                    p.Description.ToLower().Contains(filter.Query)
                );
            }

            var recordsFilters = model.Count();

            switch (filter.Sort.ToLower())
            {
                case "displayname":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.DisplayName) : model.OrderByDescending(p => p.DisplayName);
                    break;
                case "name":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.Name) : model.OrderByDescending(p => p.Name);
                    break;
                case "createtime":
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.CreateDateTime) : model.OrderByDescending(p => p.CreateDateTime);
                    break;
                default:
                    model = filter.Direction == "asc" ? model.OrderBy(p => p.CreateDateTime) : model.OrderByDescending(p => p.CreateDateTime);
                    break;
            }

            model = model.Skip(filter.Start).Take(filter.PageSize);

            var roles = _iMapper.Map<List<RoleDto>>(await model.ToListAsync());

            return new Roles
            {
                Items = roles,
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<RoleDto>> GetSelectableRolesAsync()
        {
            return _iMapper.Map<List<RoleDto>>(await _roleManager.Roles.ToListAsync());
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRole model)
        {
            var role = _iMapper.Map<AspNetRole>(model);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<AspNetRole> FindRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<AspNetRole> FindRoleByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityResult> AddUserToRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (await _userManager.IsInRoleAsync(user, role))
            {
                return IdentityResult.Success;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeCurrentRoles = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (removeCurrentRoles.Succeeded)
            {
                return await _userManager.AddToRoleAsync(user, role);
            }

            return IdentityResult.Failed(removeCurrentRoles.Errors.FirstOrDefault());
        }

        public async Task<IdentityResult> DeleteRoleAsync(DeleteRole model)
        {
            var role = await FindRoleByIdAsync(model.Id);
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(EditRole model)
        {
            var role = await FindRoleByIdAsync(model.Id);
            role.Name = model.Name;
            role.DisplayName = model.DisplayName;
            role.Description = model.Description;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<string> GetRoleByUserIdAsync(string userId)
        {
            var roles = await _userManager.GetRolesAsync(new AspNetUser { Id = userId });
            return roles.FirstOrDefault();
        }

        public async Task<AspNetRole> GetRoleModelByUserIdAsync(string userId)
        {
            var roles = await _userManager.GetRolesAsync(new AspNetUser { Id = userId });
            return await FindRoleByNameAsync(roles.FirstOrDefault());
        }

        public async Task<EditRole> GetRoleForEditAsync(string id)
        {
            var role = await FindRoleByIdAsync(id);
            return role == null || role.IsLocked ? null : _iMapper.Map<EditRole>(role);
        }

        public async Task<DeleteRole> GetRoleForDeleteAsync(string id)
        {
            var role = await FindRoleByIdAsync(id);
            return role == null || role.IsLocked ? null : _iMapper.Map<DeleteRole>(role);
        }

        public async Task<UserDto> GetUserInformationAsync(string id)
        {
            var user = _iMapper.Map<UserDto>(await FindUserByIdAsync(id));
            user.Role = await GetRoleByUserIdAsync(id);
            return user;
        }

        public async Task RefreshSignInAsync(AspNetUser user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task<List<Permission>> CurrentClaimsAsync(string roleId, List<Permission> permissions)
        {
            var role = await FindRoleByIdAsync(roleId);
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                permission.Selected = roleClaims.Any(p => p.Type == permission.Name.ToUpper());
            }

            return permissions;
        }

        public async Task ManageRoleClaimsAsync(string roleId, List<Permission> permissions)
        {
            var role = await FindRoleByIdAsync(roleId);
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                if (permission.Selected && roleClaims.All(p => p.Type != permission.Name.ToUpper()))
                {
                    await _roleManager.AddClaimAsync(role, new Claim(permission.Name.ToUpper(), true.ToString()));
                }
                else if (!permission.Selected && roleClaims.Any(p => p.Type == permission.Name.ToUpper()))
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim(permission.Name.ToUpper(), true.ToString()));
                }
            }
        }

        public async Task<bool> IsLockedOutAsync(AspNetUser user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task<IdentityResult> ResetAccessFailedCountAsync(AspNetUser user)
        {
            return await _userManager.ResetAccessFailedCountAsync(user);
        }

        public async Task SignInWithClaimsAsync(AspNetUser user)
        {
            await _signInManager.SignInAsync(user, false);
        }

        public async Task<IdentityResult> AccessFailedAsync(AspNetUser user)
        {
            return await _userManager.AccessFailedAsync(user);
        }

        public async Task<IdentityResult> LoginWithMobile(AspNetUser user, ClientInformation logInfo)
        {
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "",
                    Description = Resources.Error.MobileNotFound
                });
            }

            if (await IsLockedOutAsync(user))
            {
                await AccessFailedAsync(user);
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "",
                    Description = Resources.Error.AccountLocked
                });
            }

            await ResetAccessFailedCountAsync(user);
            await SignInWithClaimsAsync(user);

            await _logService.AddUserLogAsync(user.Id, "User", "LoggedIn", "", user, logInfo);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangeUserAccessAsync(UserDto user, bool access)
        {
            var aspnetUser = await FindUserByIdAsync(user.Id);
            aspnetUser.IsBlocked = !access;
            var updateUser = await _userManager.UpdateAsync(aspnetUser);
            if (updateUser.Succeeded)
            {
                var fullName = aspnetUser.Name;
                if (aspnetUser.IsBlocked)
                {
                    await _logService.AddUserLogAsync(user.LogUserId, "User", "BlockUser", fullName, user, user.LogInfo);
                }
                else
                {
                    await _logService.AddUserLogAsync(user.LogUserId, "User", "UnblockUser", fullName, user, user.LogInfo);
                }
            }
            return updateUser;
        }

        public async Task<int> NumberOfCustomersAsync()
        {
            var allUserRoles = await _userManager.GetUsersInRoleAsync("Customer");
            return allUserRoles.Count;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePassword model)
        {
            var user = await FindUserByIdAsync(model.Id);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPassword model)
        {
            var user = await FindUserByIdAsync(model.Id);
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return await _userManager.AddPasswordAsync(user, model.NewPassword);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        }

        public async Task<List<UserDto>> GetPersonalChefAsync()
        {
            var pChefs = await _userManager.GetUsersInRoleAsync("PersonalChef");
            return _iMapper.Map<List<UserDto>>(pChefs);
        }

        public async Task<IdentityResult> SetDefaultKitchenAsync(string userId, int kitchenId)
        {
            var user = await FindUserByIdAsync(userId);
            user.KitchenId = kitchenId;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ServiceResult> SwitchRoleAsync(SwitchRole model)
        {
            var user = await FindUserByIdAsync(model.UserId);
            if (user != null)
            {
                var currentRole = await GetRoleByUserIdAsync(model.UserId);
                if (currentRole != null)
                {
                    var addStatus = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (addStatus.Succeeded)
                    {
                        var removeStatus = await _userManager.RemoveFromRoleAsync(user, currentRole);
                        if (removeStatus.Succeeded)
                        {
                            return new ServiceResult(true);
                        }
                    }
                    await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                }
            }
            return new ServiceResult(false);
        }
    }
}
