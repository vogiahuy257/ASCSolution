using ASCWeb.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ASCWeb.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<ApplicationSettings> options)
        {
            if (options == null || options.Value == null)
                throw new ArgumentNullException(nameof(options), "ApplicationSettings không được null.");

            var settings = options.Value;

            // 🔹 Tạo danh sách roles từ ApplicationSettings
            var roles = settings.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(r => r.Trim())
                                      .Distinct()
                                      .ToList();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Không thể tạo role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // 🔹 Tạo tài khoản Admin nếu chưa có
            await CreateUserIfNotExists(userManager, settings.AdminEmail, settings.AdminName, settings.AdminPassword, "Admin");

            // 🔹 Tạo tài khoản Engineer nếu chưa có
            await CreateUserIfNotExists(userManager, settings.EngineerEmail, settings.EngineerName, settings.EngineerPassword, "Engineer");
        }

        private async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, string email, string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"Thông tin {role} không hợp lệ trong ApplicationSettings.");

            email = email.Trim().ToLower();
            username = username.Trim();

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                    NormalizedEmail = email.ToUpper(), // Chuẩn hóa email
                    NormalizedUserName = username.ToUpper(),
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    await userManager.AddClaimsAsync(user, new[]
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim("IsActive", "True")
                    });
                }
                else
                {
                    throw new Exception($"Lỗi tạo {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
