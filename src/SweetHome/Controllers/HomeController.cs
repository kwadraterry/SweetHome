using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;

namespace SweetHome.Controllers
{
    public class HomeController : Controller
    {
        [FromServices]
        public ApplicationDbContext DbContext { get; set; }
        
        public IActionResult Index()
        {
            var animals = DbContext.ShelterAnimals
                .Include(a => a.Shelter)
                .Take(5)
                .ToList();
            return View(animals);
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