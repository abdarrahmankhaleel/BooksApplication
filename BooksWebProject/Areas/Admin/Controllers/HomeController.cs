using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Permissions.Home.View)]
    public class HomeController : Controller
    {
        [Authorize(Permissions.Home.View)]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View();
        }
    }
}
