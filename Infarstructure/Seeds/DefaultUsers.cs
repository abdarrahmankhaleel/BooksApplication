//using static Domin.Entity.Helper; عشان بدل ما اظلك تكتب هلبر دوت
using Domain.Constants;
using Domin.Entity;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var Default = new ApplicationUser()
            {
                UserName = Helper.UserNameBasic,
                Email = Helper.EmailBasic,
                Name = Helper.NameBasic,
                ImageUser = "user1.jpg",
                ActiveUser = true,
                EmailConfirmed = true
            };

            var user = userManager.FindByEmailAsync(Default.Email);
            if (user.Result == null)
            {
                await userManager.CreateAsync(Default, Helper.PasswordBasic);
                await userManager.AddToRolesAsync(Default, new List<string> { Helper.eRoles.Basic.ToString(), });
            }

        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var Default = new ApplicationUser()
            {
                UserName = Helper.UserName,
                Email = Helper.Email,
                Name = Helper.Name,
                ImageUser = "user1.jpg",
                ActiveUser = true,
                EmailConfirmed = true
            };

            var user =await userManager.FindByEmailAsync(Default.Email);//ممكن هنا بدون ما تحط اويت بس لازم تحط يوزر دوت ريزلت
            if (user == null)
            {
                await userManager.CreateAsync(Default, Helper.Password);
                await userManager.AddToRolesAsync(Default, new List<string> { Helper.eRoles.SuperAdmin.ToString(), });
            }

            //Code for Seeding claims
            await roleManager.SeedClaimsAsync();

        }


        public static async Task SeedClaimsAsync(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Helper.eRoles.SuperAdmin.ToString());
            //Code Add Permission Claims
            var modules = Enum.GetValues(typeof(Helper.PermissionModuleName));
            foreach (var module in modules)
            {
                await roleManager.AddPermissionsClaims(adminRole,module.ToString());
            }
        }

        public static async Task AddPermissionsClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsModule(module);

            foreach (var permission in allPermissions)
            {
                //لو مافيش ولا صف من صفوف جدول الكليمز التايب بتاعه يساوي دا والفاليو بتاعته دي 
                if (!allClaims.Any(x => x.Type == Helper.Permission && x.Value == permission))
                    await roleManager.AddClaimAsync(role,new Claim(Helper.Permission,permission)); 
            }

        }

    }
}
