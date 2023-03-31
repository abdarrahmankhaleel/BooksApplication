using Domin.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.Seeds
{
    public static class DefaultRoles
    {
        //هيكريتلي الرولز دي اوتوماتيك بمجرد ما انده على الميثود دي في كلاس البرقرم
        // في كلاس البرقرم في دالة المين بين بين كريت الهوت ورنها
        //ولكن هيشك اول حاجة قبل ما يكريت اي رول عاوز لو اي رول من دول موجودة قبل كدا ما يكريتهاش ثاني
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
         //   if (!roleManager.Roles.Any())//الافاية بتفقول انه لو مفيش ولا رول عندي في الداتا بيز هيبقى ضيفلي الرولز دي وهو اصلا عندي رول فمش هيضيفلي حاجة 
         //ثانيا اي يوزر اله صلاحية او برمشن انو يحذف اي رول من الرولز الحالية فمش هيضيف الرول الناقصة
           // {
            await roleManager.CreateAsync(new IdentityRole(Helper.eRoles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Helper.eRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Helper.eRoles.Basic.ToString()));
          //  }
        }
    }
}
