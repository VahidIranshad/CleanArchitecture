using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CA.Application.Contracts.Identity;
using CA.Domain.Constants.Identity;
using CA.Identity.Models;
using System.Text;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using Fop.FopExpression;
using Fop;

namespace CA.Identity.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            IMapper mapper,
            RoleManager<Role> roleManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponse>>(users);
            return result;
        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public async Task RegisterAsync(RegisterRequest request, string origin)
        {
            if (IsValidEmail(request.UserName) == false)
            {
                throw new Exception("Email is invalid");
            }
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new Exception(string.Format("Username {0} is already taken.", request.UserName));
            }
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                //IsActive = request.ActivateUser,
                //EmailConfirmed = request.AutoConfirmEmail
                IsActive = true,
                EmailConfirmed = true
            };

            //if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            //{
            //    var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            //    if (userWithSamePhoneNumber != null)
            //    {
            //        throw new Exception(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
            //        //return await Result.FailAsync(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
            //    }
            //}

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.DefaultRole);
                }
                else
                {
                    throw new Exception(String.Join(',', result.Errors.Select(a => a.Description.ToString()).ToList()));
                    //return await Result.FailAsync(result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
                }
            }
            else
            {
                throw new Exception(string.Format("Email {0} is already registered.", request.Email));
            }
        }

        private async Task<string> SendVerificationEmail(User user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/identity/user/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<UserResponse> GetAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            return result;
        }

        public async Task ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
            if (isAdmin)
            {
                throw new Exception("Administrators Profile's Status cannot be toggled");
            }
            if (user != null)
            {
                user.IsActive = request.ActivateUser;
                var identityResult = await _userManager.UpdateAsync(user);
            }
        }

        public async Task<List<UserRoleModel>> GetRolesAsync(string userId)
        {
            var result = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var userRole in userRoles)
            {
                var role = roles.First(p => p.Name == userRole);
                var userRolesViewModel = new UserRoleModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleDescription = role.Description
                };
                result.Add(userRolesViewModel);
            }
            return result;
        }

        public async Task InsertUserRolesAsync(UpdateUserRolesRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user.Email.ToLower() == "admin@localhost.com")
                {
                    throw new Exception("Not Allowed.");
                }

                var roles = await _userManager.GetRolesAsync(user);

                var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
                {
                    var tryToAddAdministratorRole = request.RoleID == RoleConstants.AdministratorRole;
                    var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
                    if (tryToAddAdministratorRole && !userHasAdministratorRole)
                    {
                        throw new Exception("Not Allowed to add or delete Administrator Role if you have not this role.");
                    }
                }

                var result = await _userManager.AddToRolesAsync(user, new List<string> { _roleManager.Roles.First(p => p.Id == request.RoleID).Name });
                if (result.Errors != null && result.Errors.Any())
                {
                    throw new Exception(String.Join(",", result.Errors.Select(p => p.Description).ToList()));
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public async Task RemoveUserRoleAsync(UpdateUserRolesRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user.Email.ToLower() == "admin@localhost.com")
                {
                    throw new Exception("Not Allowed.");
                }

                var roles = await _userManager.GetRolesAsync(user);

                var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
                {
                    var tryToAddAdministratorRole = request.RoleID == RoleConstants.AdministratorRole;
                    var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
                    if (tryToAddAdministratorRole && !userHasAdministratorRole)
                    {
                        throw new Exception("Not Allowed to add or delete Administrator Role if you have not this role.");
                    }
                }

                var result = await _userManager.RemoveFromRoleAsync(user, _roleManager.Roles.First(p => p.Id == request.RoleID).Name);
                if (result.Errors != null && result.Errors.Any())
                {
                    throw new Exception(String.Join(",", result.Errors.Select(p => p.Description).ToList()));
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public async Task<string> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return string.Format("Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.", user.Email);
            }
            else
            {
                throw new Exception(string.Format("An error occurred while confirming {0}", user.Email));
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new Exception("An Error has occurred!");
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            //var mailRequest = new MailRequest
            //{
            //    Body = string.Format(_localizer["Please reset your password by <a href='{0}'>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL)),
            //    Subject = _localizer["Reset Password"],
            //    To = request.Email
            //};
            //BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.ID);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new Exception("An Error has occured!");
            }

            var result = await _userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
            }
            else
            {
                throw new Exception("An Error has occured!");
            }
            var passs = await _userManager.GeneratePasswordResetTokenAsync(user);
            result = await _userManager.AddPasswordAsync(user, "P@ssword1");
            if (result.Succeeded)
            {
            }
            else
            {
                throw new Exception("An Error has occured!");
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _userManager.Users.CountAsync();
            return count;
        }

        public async Task<(List<UserResponse>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true)
        {

            var fopRequest = FopExpressionBuilder<User>.Build(Filter, Order, PageNumber ?? 0, PageSize ?? 0);
            IQueryable<User> query = this._userManager.Users.AsQueryable();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            var (datas, count) = query.ApplyFop(fopRequest);
            var list = _mapper.Map<List<UserResponse>>(await datas.ToListAsync());
            return (list, count);
        }

    }
}
