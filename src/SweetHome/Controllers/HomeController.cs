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
            var shelters = DbContext.Shelters.Where(s => s.Name == "ABC").ToList();
            var animals = DbContext.ShelterAnimals
                .Where(a => a.IsHappy == false)
                .Include(a => a.Shelter)
                .Take(5)
                .ToList();
            return View();
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