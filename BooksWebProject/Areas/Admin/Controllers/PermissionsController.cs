using Domain.Constants;
using Domin.Entity;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBook.Areas.Admin.Controllers;

namespace BooksWebProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Permissions.Roles.Delete)]
    public class PermissionsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionsController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissionList = Permissions.PermissionsList();
           var allPermissionIen = allPermissionList.Select(x=>new RoleClaimsViewModel {Value=x });
           var allPermissions= allPermissionIen.ToList();    
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            model.RoleId=roleId;
            model.RoleName = role.Name;
            return View(model);
            //var role =await _roleManager.FindByIdAsync(roleId);
            //var claims =await _roleManager.GetClaimsAsync(role);
            //var allPermissions = Permissions.PermissionsList();
            //var x = allPermissions.Select(x => new RoleClaimsViewModel { Value = x }).ToList();
            //return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims=await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
               await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(c => c.Selected).ToList();

            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(Helper.Permission,claim.Value));
            }
            return RedirectToAction("Roles", nameof(AccountsController));
        }
    }
}
