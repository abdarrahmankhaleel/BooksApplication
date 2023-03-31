using Infarstructure.IRepository;
using Microsoft.AspNetCore.Mvc;
using Domin.Entity;
using Microsoft.AspNetCore.Identity;
using Infarstuructre.ViewModel;
using System;
using Microsoft.AspNetCore.Http;
using BooksWebProject.Resource;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.ViewModel;

namespace BooksWebProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin,SupperAdmin")]

    public class SubCategoriesController : Controller
    {
        private void SessionMsg(string Type, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, Type);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
        private readonly IServicesRepository<SubCategory> servicesSubCategory;
        private readonly IServesisRepositoryLog<LogSubCategory> servicesLogSubCategory;
        private readonly IServicesRepository<Category> servicesCategory;
        private readonly UserManager<ApplicationUser> userManager;

        public SubCategoriesController(IServicesRepository<SubCategory> servicesSubCategory
            , IServesisRepositoryLog<LogSubCategory> servicesLogSubCategory
            , IServicesRepository<Category> servicesCategory
            , UserManager<ApplicationUser> userManager)
        {
            this.servicesSubCategory = servicesSubCategory;
            this.servicesLogSubCategory = servicesLogSubCategory;
            this.servicesCategory = servicesCategory;
            this.userManager = userManager;
        }
        
        public IActionResult SubCategories()
        {
            ViewModelSubCategory subCategoryViewModel = new ViewModelSubCategory()
            {
                lstSubCategories = servicesSubCategory.GetAll(),
                lstLogSubCategories = servicesLogSubCategory.GetAll(),
                SubCategory = new SubCategory()
            };
            ViewBag.lstCategories = servicesCategory.GetAll();
            return View(subCategoryViewModel);
        }
        public IActionResult Save(ViewModelSubCategory viewModel)//مين نده على الاكشن دي الفورمة والفرمة مبتعثش غير محتواها
        {
            var userId = userManager.GetUserId(User);
            if (viewModel.SubCategory.Id != Guid.Empty)//edit
            {
                if (servicesSubCategory.Save(viewModel.SubCategory))
                {
                    servicesLogSubCategory.Update(viewModel.SubCategory.Id, (Guid.Parse(userId)));
                    SessionMsg(Helper.Success, ResourceWeb.lbEdit, ResourceWeb.lbMsgEditSubCategory);
                }
                else
                    SessionMsg(Helper.Error, ResourceWeb.lbNotUpdate, ResourceWeb.lbMsgNotSaveNewCategory);
            }
            else//add
            {
                var SameName = servicesSubCategory.FindBy(viewModel.SubCategory.Name);
                if (SameName != null)
                {
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgDuplicatName);
                    return RedirectToAction(nameof(SubCategories));
                }
                if (servicesSubCategory.Save(viewModel.SubCategory))
                {
                    servicesLogSubCategory.Save(viewModel.SubCategory.Id, (Guid.Parse(userId)));
                    SessionMsg(Helper.Success, ResourceWeb.lbSave, ResourceWeb.lbMsgSaveNewSubCategory);
                }
                else
                {
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgNotSaveNewCategory);
                }
            }
           
            return RedirectToAction(nameof(SubCategories));
        }


        public IActionResult Delete(Guid id)
        {
            var userId = userManager.GetUserId(User);

            if (servicesSubCategory.Delete(id))
                servicesLogSubCategory.Delete(id, (Guid.Parse(userId)));

            return RedirectToAction(nameof(SubCategories));
        }
        public IActionResult DeleteLog(Guid id)
        {
            if(servicesLogSubCategory.DeleteLog(id))
                return RedirectToAction(nameof(SubCategories));
            return RedirectToAction(nameof(SubCategories));

            
        }

    }
}
