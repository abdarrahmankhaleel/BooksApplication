using BooksWebProject.Resource;
using Domain.Constants;
using Domin.Entity;
using Infarstuructre.Data;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Permissions.Accounts.View)]
    public class AccountsController : Controller
    {
        #region Decleration
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FreeBookDbContext _context;
        #endregion

        #region Constructor
        public AccountsController(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            FreeBookDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        #endregion

        #region Method
        [Authorize(Permissions.Roles.View)]
        public IActionResult Roles()
        {
            var viewMod = new RolesViewModel()
            {
                NewRole = new NewRole(),
                Roles = _roleManager.Roles.OrderBy(x => x.Name).ToList()
            };
            return View(viewMod);
        }
        [Authorize(Permissions.Roles.Delete)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var ruselt = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            if (ruselt != null)
            {
                if((await _roleManager.DeleteAsync(ruselt)).Succeeded)
                {
                    return RedirectToAction("Roles");
                }    
            }
            return RedirectToAction("Roles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Roles.Create)]
        public async Task<IActionResult> Roles(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Id = model.NewRole.RoleId,
                    Name = model.NewRole.RoleName
                };
                //create
                if (role.Id == null)
                {
                    role.Id = Guid.NewGuid().ToString();
                    var result =await _roleManager.CreateAsync(role);
                    if (result.Succeeded)// Succeeded
                        SessionMsg(Helper.Success, ResourceWeb.lbSave,ResourceWeb.lbSaveMsgRole);
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbNotSavedMsgRole);
                }
                //update
                else
                {
                    var RoleUpdate=await _roleManager.FindByIdAsync(role.Id);
                    RoleUpdate.Id = model.NewRole.RoleId;
                    RoleUpdate.Name = model.NewRole.RoleName;
                    var result = await _roleManager.UpdateAsync(RoleUpdate);
                    if (result.Succeeded)
                        SessionMsg(Helper.Success, ResourceWeb.lbUpdate, ResourceWeb.lbNotUpdateMsgRole);
                     SessionMsg(Helper.Error, ResourceWeb.lbNotUpdate, ResourceWeb.lbNotUpdateMsgRole);   
                }

            }
            return RedirectToAction(nameof(Roles));
        }

        private void SessionMsg(string Type,string Title,string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, Type);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
        //public IActionResult Register()
        //{
        //    var model = new RegisterViewModel
        //    //Users = _userManager.Users.in.OrderBy(x => x.Name).ToList() ;
        //    {// VwUsers is being in context if it's not existing in it then would go error
        //     // and also it's must be the same datatype 
        //        Users = _context.VwUsers.OrderBy(x => x.Role).ToList(),//Users in obj model from RegisterViewModel class ..Users has the same datatype as VwUsers in context which is class model VwUser.cs// هاي لست اليوزرس
        //        NewRegister =new NewRegister(),// الفورمة
        //        Roles = _roleManager.Roles.OrderBy(x=>x.Name).ToList(),// عشان الدروب داةن ليست في الفورمة 
        //    };
        //    return View(model);
        //}

        [Authorize(Permissions.Accounts.View)]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                NewRegister = new NewRegister(),
                Roles = _roleManager.Roles.OrderBy(x => x.Name).ToList(),
                Users = _context.VwUsers.OrderBy(x => x.Role).ToList() //_userManager.Users.OrderBy(x=>x.Name).ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Accounts.Create)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count() > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                    var fileStream = new FileStream(Path.Combine(@"wwwroot/", Helper.PathSaveImageuser, ImageName), FileMode.Create);
                    file[0].CopyTo(fileStream);
                    model.NewRegister.ImageUser = ImageName;
                }
                else
                {
                    model.NewRegister.ImageUser = model.NewRegister.ImageUser;
                }
                var user = new ApplicationUser
                {
                    Id=model.NewRegister.Id,
                    Name=model.NewRegister.Name,    
                    Email=model.NewRegister.Email,
                    UserName=model.NewRegister.Email,
                    ImageUser=model.NewRegister.ImageUser,
                    ActiveUser=model.NewRegister.ActiveUser,
                    
                };
                if (user.Id==null)
                {
                    //CREATE
                    user.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(user, model.NewRegister.Password);
                    if (result.Succeeded)// السكسيدد مش هتنعمل غير تتعمل اويت
                    {
                        ///seccessed  
                        ///
                        var Role = await _userManager.AddToRoleAsync(user, model.NewRegister.RoleName);
                        if (Role.Succeeded)
                        {

                            // Succeeded
                            HttpContext.Session.SetString("msgType", "seccess");
                            HttpContext.Session.SetString("msgTitle", ResourceWeb.lbSave);
                            HttpContext.Session.SetString("msg", ResourceWeb.lbSaveMsgUser);
                        }
                        else
                        {
                            HttpContext.Session.SetString("msgType", "error");
                            HttpContext.Session.SetString("msgTitle", ResourceWeb.lbNotSaved);
                            HttpContext.Session.SetString("msg", ResourceWeb.lbNotSavedMsgRole);
                        }

                    }
                    else
                    {
                        //not secceseedd
                    }
                }
                else
                {
                    //update
                    var userUpdate =await _userManager.FindByIdAsync(model.NewRegister.Id);
                    userUpdate.Id = model.NewRegister.Id;
                    userUpdate.Name = model.NewRegister.Name;
                    userUpdate.Email = model.NewRegister.Email;
                    userUpdate.UserName = model.NewRegister.Email;
                    userUpdate.ImageUser = model.NewRegister.ImageUser; 
                    userUpdate.ActiveUser = model.NewRegister.ActiveUser;
                    var result =await _userManager.UpdateAsync(userUpdate);
                    if (result.Succeeded)
                    {
                        //seeccedd update
                        var oldRole = await _userManager.GetRolesAsync(userUpdate);
                        await _userManager.RemoveFromRolesAsync(userUpdate, oldRole);
                      var AddRole=  await _userManager.AddToRoleAsync(userUpdate,model.NewRegister.RoleName);
                        if (AddRole.Succeeded)
                        {
                            HttpContext.Session.SetString("msgType", "seccess");
                            HttpContext.Session.SetString("msgTitle", ResourceWeb.lbUpdate);
                            HttpContext.Session.SetString("msg", ResourceWeb.lbUpdateMsgUser);
                        }
                        else
                        {
                            HttpContext.Session.SetString("msgType", "error");
                            HttpContext.Session.SetString("msgTitle", ResourceWeb.lbNotUpdate);
                            HttpContext.Session.SetString("msg", ResourceWeb.lbNotUpdateMsgRole);
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString("msgType", "error");
                        HttpContext.Session.SetString("msgTitle", ResourceWeb.lbNotUpdate);
                        HttpContext.Session.SetString("msg", ResourceWeb.lbNotUpdateMsgUser);
                    }
                }
                return RedirectToAction("Register", "Accounts");
            }
            return RedirectToAction("Register", "Accounts");

        }


        [Authorize(Permissions.Accounts.Delete)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user=_userManager.Users.FirstOrDefault(x=>x.Id== userId);

            if (user != null)
            {
                if(user.ImageUser!=null && user.ImageUser != Guid.Empty.ToString())
                {
                    var pathImg = Path.Combine(@"wwwroot/", Helper.PathSaveImageuser, user.ImageUser);
                    if (System.IO.File.Exists(pathImg))
                    {
                        System.IO.File.Delete(pathImg);
                    }
                }
              if((await _userManager.DeleteAsync(user)).Succeeded)
                {
                    return RedirectToAction("Register", "Accounts");

                }
                else
                {
                   

                }
                return RedirectToAction("Register", "Accounts");
            }
            return RedirectToAction("Register", "Accounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Accounts.Create)]
        public async Task<IActionResult> ChangePassword(RegisterViewModel model)
        {
            var user=await _userManager.FindByIdAsync(model.ChangePassword.Id);
            if (user != null)
            {
               await  _userManager.RemovePasswordAsync(user);
              var ruseult=await _userManager.AddPasswordAsync(user, model.ChangePassword.NewPassword);
                if (ruseult.Succeeded)
                {
                    // Succeeded
                    HttpContext.Session.SetString("msgType", Helper.Success);
                    HttpContext.Session.SetString("msgTitle", ResourceWeb.lbSave);
                    HttpContext.Session.SetString("msg", ResourceWeb.lbMsgSavedChangePassword);
                }
                else
                {
                    HttpContext.Session.SetString("msgType", "error");
                    HttpContext.Session.SetString("msgTitle", ResourceWeb.lbNotSaved);
                    HttpContext.Session.SetString("msg", ResourceWeb.lbMsgNotSavedChangePassword);
                }
                return RedirectToAction(nameof(Register));
            }
            return RedirectToAction(nameof(Register));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
           var result=await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            }
            ViewBag.errorMessge=false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToAction(nameof(Login));    

        }

    #endregion 
    }
}
