namespace WebStudy.Helpers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebStudy.Data.Entities;
    using WebStudy.ViewModels;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string rolName)
        {
            await this.userManager.AddToRoleAsync(user, rolName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await this.userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await this.userManager.ConfirmEmailAsync(user, token);
        }



        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string rolName)
        {
            return await this.userManager.IsInRoleAsync(user, rolName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await this.signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
            ///El parametro LockuoutOnFailure si esta en true bloquea la cuenta de usaurio despues de n(paramatetrizable en el Starup.cs) intentos fallidos
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await this.userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await this.userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await this.signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
            ///El parametro LockuoutOnFailure si esta en true bloquea la cuenta de usuario despues de n(paramatetrizable en el Starup.cs) intentos fallidos
        }

        public async Task DeleteUserAsync(User user)
        {
            await this.userManager.DeleteAsync(user);
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }
        public async Task<List<User>> GetAllUserAsync()
        {
            return await this.userManager.Users
                .Include(u => u.City)
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName)
                .ToListAsync();
        }
    }
}
