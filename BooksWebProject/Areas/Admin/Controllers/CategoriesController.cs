using BooksWebProject.Resource;
using Domin.Entity;
using Infarstructure.IRepository;
using Infarstructure.ViewModel;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BooksWebProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SupperAdmin")]
    [Area(nameof(Admin))]
    public class CategoriesController : Controller
    {
        private readonly IServicesRepository<Category> servicesCategory;
        private readonly IServesisRepositoryLog<LogCategory> servesisCategoryLog;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriesController(IServicesRepository<Category> servicesCategory,
            IServesisRepositoryLog<LogCategory> servesisCategoryLog,
            UserManager<ApplicationUser> userManager)
        {
            this.servicesCategory = servicesCategory;
            this.servesisCategoryLog = servesisCategoryLog;
            this.userManager = userManager;
        }
        public IActionResult Categories() 
        {
            return View(new CategoryViewModel
            {
                Categories = servicesCategory.GetAll(),
                LogCategories = servesisCategoryLog.GetAll(),
                NewCategory = new Category()
            }); 
        }

        public IActionResult Delete(Guid id)
        { 
            var userId=userManager.GetUserId(User);
           if(servicesCategory.Delete(id) && servesisCategoryLog.Delete(id,Guid.Parse(userId)))
                return RedirectToAction("Categories");
            return RedirectToAction("Categories");

        }
        public IActionResult DeleteLog(Guid id)
        { 
           if(servesisCategoryLog.DeleteLog(id))
                return RedirectToAction("Categories");
            return RedirectToAction("Categories");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CategoryViewModel model)
        { 
            var userId=userManager.GetUserId(User);
            if (model.NewCategory.Id == Guid.Empty)
            {//create
                if(servicesCategory.FindBy(model.NewCategory.Name) != null)
                {
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgDuplicatNameCategory); 
                }
                else
                {
                    if (servicesCategory.Save(model.NewCategory)
                        && servesisCategoryLog.Save(model.NewCategory.Id,Guid.Parse(userId)))
                        SessionMsg(Helper.Success, ResourceWeb.lbSave, ResourceWeb.lbMsgSaveNewCategory);
                    else
                        SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgNotSaveNewCategory);
                }
            }
            else
            {//edit//update
                if (servicesCategory.Save(model.NewCategory) && servesisCategoryLog.Update(model.NewCategory.Id, Guid.Parse(userId)))
                    SessionMsg(Helper.Success, ResourceWeb.lbEdit, ResourceWeb.lbMsgEditCategory);
                else
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgNotEditCategory);
            }
            return RedirectToAction("Categories");
        }
        private void SessionMsg(string Type, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, Type);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }

    }
}
