using Infarstructure.IRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Domin.Entity;
using Microsoft.AspNetCore.Identity;
using Infarstuructre.ViewModel;
using System;
using Microsoft.AspNetCore.Http;
using BooksWebProject.Resource;
using Microsoft.AspNetCore.Authorization;
using Infarstructure.IRepository.ServicesRepository;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace BooksWebProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin,SupperAdmin")]

    public class BooksController : Controller
    {
        private void SessionMsg(string Type, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, Type);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
        private readonly IServicesRepository<SubCategory>_servicesSubCategory;
        private readonly IServesisRepositoryLog<LogBook> _servicesLogBook;
        private readonly IServicesRepository<Category>   _servicesCategory;
        private readonly IServicesRepository<Book>       _servicesBook;
        private readonly UserManager<ApplicationUser>    _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BooksController(IServicesRepository<SubCategory> servicesSubCategory
            , IServesisRepositoryLog<LogBook> servicesLogBook
            , IServicesRepository<Category> servicesCategory
            , IServicesRepository<Book> servicesBook
            , UserManager<ApplicationUser> userManager
            , IWebHostEnvironment hostEnvironment)
        {
            _servicesSubCategory = servicesSubCategory;
            _servicesLogBook = servicesLogBook;
            _servicesCategory = servicesCategory;
            _servicesBook = servicesBook;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Books()
        {
            ViewModelBooks subCategoryViewModel = new ViewModelBooks()
            {
                 LstBooks= _servicesBook.GetAll(),
                LstLogBooks = _servicesLogBook.GetAll(),
                Book = new Book(),
            };
            ViewBag.lstCategories = _servicesCategory.GetAll();
            return View(subCategoryViewModel);
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
        public async Task<IActionResult> Save(ViewModelBooks viewModel)//مين نده على الاكشن دي الفورمة والفرمة مبتعثش غير محتواها
        {
            if (viewModel.CoverImage != null && viewModel.CoverImage.Length > 0)
            {
                var uniqueFileName = GetUniqueFileName(viewModel.CoverImage.FileName);
                var uploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.CoverImage.CopyToAsync(stream);
                }
                viewModel.Book.ImageName = uniqueFileName;
            }
            var userId = _userManager.GetUserId(User);
            if (viewModel.Book.Id != Guid.Empty)//edit
            {
                if (_servicesBook.Save(viewModel.Book))
                {
                    _servicesLogBook.Update(viewModel.Book.Id, (Guid.Parse(userId)));
                    SessionMsg(Helper.Success, ResourceWeb.lbEdit, ResourceWeb.lbMsgEditBook);
                }
                else
                    SessionMsg(Helper.Error, ResourceWeb.lbNotUpdate, ResourceWeb.lbMsgNotSaveBook);
            }
            else//add
            {
                var SameName = _servicesBook.FindBy(viewModel.Book.Name);
                if (SameName != null)
                {
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgDuplicatName);
                    return RedirectToAction(nameof(Books));
                }
                if (_servicesBook.Save(viewModel.Book))
                {
                    _servicesLogBook.Save(viewModel.Book.Id, (Guid.Parse(userId)));
                    SessionMsg(Helper.Success, ResourceWeb.lbSave, ResourceWeb.lbMsgSaveNewBook);
                }
                else
                {
                    SessionMsg(Helper.Error, ResourceWeb.lbNotSaved, ResourceWeb.lbMsgNotSaveBook);
                }
            }

            return RedirectToAction(nameof(Books));
        }


        public IActionResult Delete(Guid id)
        {
            var userId = _userManager.GetUserId(User);

            if (_servicesBook.Delete(id))
                _servicesLogBook.Delete(id, (Guid.Parse(userId)));

            return RedirectToAction(nameof(Books));
        }
        public IActionResult DeleteLog(Guid id)
        {
            if (_servicesLogBook.DeleteLog(id))
                return RedirectToAction(nameof(Books));
            return RedirectToAction(nameof(Books));


        }


        [HttpGet]
        public IActionResult GetSubcategories(Guid categoryId)
        {
            // Retrieve the subcategories from your data source based on the selected category ID.
            var subcategories = _servicesBook.GetSubcategoriesByCategoryId(categoryId);

            // Return the subcategories as JSON.
            return Json(subcategories);
        }

    }
}
