using ASCWeb.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASCWeb.Data
{
    public interface IIdentitySeed
    {
        Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<ApplicationSettings> options);
    }
}
