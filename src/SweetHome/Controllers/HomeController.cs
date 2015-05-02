using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;

namespace SweetHome.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var shelters = context.Shelters.Where(s => s.Name == "ABC").ToList();
            return View(shelters);
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}