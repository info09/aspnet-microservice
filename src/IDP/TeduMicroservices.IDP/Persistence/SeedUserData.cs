using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TeduMicroservices.IDP.Common;
using TeduMicroservices.IDP.Entities;

namespace TeduMicroservices.IDP.Persistence
{
    public class SeedUserData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<TeduIdentityContext>(opt => opt.UseSqlServer(connectionString));
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<TeduIdentityContext>()
                .AddDefaultTokenProviders();

            using(var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    CreateUser(scope, "admin", "admin", "HaNoi", Guid.NewGuid().ToString(), "admin123", "Admin", "admin@gmail.com");
                }
            }
        }

        private static void CreateUser(IServiceScope scope, string firstName, string lastName, string address, string id, string password, string role, string email)
        {
            var userManagement = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user = userManagement.FindByNameAsync(email).Result;
            if (user == null)
            {
                user = new User
                {
                    Id = id,
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    EmailConfirmed = true
                };
                var result = userManagement.CreateAsync(user, password).Result;
                CheckResult(result);

                var addToRoleResult = userManagement.AddToRoleAsync(user, role).Result;
                CheckResult(addToRoleResult);

                result = userManagement.AddClaimsAsync(user, new List<Claim>
                {
                    new Claim(SystemConstants.Claims.UserName, user.UserName),
                    new Claim(SystemConstants.Claims.FirstName, firstName),
                    new Claim(SystemConstants.Claims.LastName, lastName),
                    new Claim(JwtClaimTypes.Address, address),
                    new Claim(SystemConstants.Claims.Roles, role),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                }).Result;

                CheckResult(result);
            }
        }

        private static void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }
}
