using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using CA.Domain.Constants.Identity;
using CA.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CA.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User Not Found.");
            }

            var identityResult = await this._userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);
            var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request, string userId)
        {

            var editor = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var isAdmin = await _userManager.IsInRoleAsync(editor, RoleConstants.AdministratorRole);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User Not Found.");
            }
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail != user.NormalizedEmail && x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    throw new Exception(string.Format("Phone number {0} is already used.", request.PhoneNumber));
                }
            }
            if ((user.Id == userId || isAdmin))
            {
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (request.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                }
                var identityResult = await _userManager.UpdateAsync(user);
                var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
                await _signInManager.RefreshSignInAsync(user);
            }
            else
            {
                throw new Exception(string.Format("Email {0} is already used.", request.Email));
            }
        }

        public async Task<string> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {;
                throw new Exception("User Not Found");
            }
            return user.ProfilePictureDataUrl;
        }
    }
}