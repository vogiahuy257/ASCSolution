using ASCWeb.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASCWeb.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<ApplicationSettings> options)
        {
            var settings = options.Value;

            // Tạo danh sách roles
            var roles = settings.Roles.Split(',');

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Trim()))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role.Trim()));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Không thể tạo role: {role}");
                    }
                }
            }

            // Tạo Admin nếu chưa có
            if (await userManager.FindByEmailAsync(settings.AdminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = settings.AdminName,
                    Email = settings.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, settings.AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddClaimsAsync(admin, new[]
                    {
                        new Claim(ClaimTypes.Email, settings.AdminEmail),
                        new Claim("IsActive", "True")
                    });
                }
                else
                {
                    throw new Exception("Không thể tạo tài khoản Admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Tạo Engineer nếu chưa có
            if (await userManager.FindByEmailAsync(settings.EngineerEmail) == null)
            {
                var engineer = new IdentityUser
                {
                    UserName = settings.EngineerName,
                    Email = settings.EngineerEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(engineer, settings.EngineerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(engineer, "Engineer");
                    await userManager.AddClaimsAsync(engineer, new[]
                    {
                        new Claim(ClaimTypes.Email, settings.EngineerEmail),
                        new Claim("IsActive", "True")
                    });
                }
                else
                {
                    throw new Exception("Không thể tạo tài khoản Engineer: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
