using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;

namespace SweetHome.Controllers
{
    public class HomeController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public IActionResult Volunteers()
        {
            return View();
        }
        
        public IActionResult Animals()
        {
            return View();
        }

        public IActionResult Shelters()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Shelter()
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