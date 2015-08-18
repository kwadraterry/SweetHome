using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;
using NHibernate;

namespace SweetHome.Controllers
{
    public class HomeController : Controller
    {   
        private readonly ISessionFactory sessionFactory;
        public HomeController(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
        public IActionResult Index()
        {
            ViewBag.PageAction = "Index";
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        
        public IActionResult Volunteers()
        {
            ViewBag.PageAction = "Volunteers";
            return View();
        }
        
        public IActionResult Animals()
        {
            ViewBag.PageAction = "Animals";
            if (Context.Request.Query.ContainsKey("animal_id"))
            {
                int animalId;
                if (Int32.TryParse(Context.Request.Query["animal_id"], out animalId))
                {
                    ViewBag.AnimalId = animalId;
                }
                else
                {
                    ViewBag.AnimalId = null;
                }
            }
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var animals = session.QueryOver<ShelterAnimal>();
                if (Context.Request.Query.ContainsKey("type"))
                {
                    AnimalType animalType;
                    if (Enum.TryParse(Context.Request.Query["type"], out animalType))
                        animals = animals.Where(animal => animal.AnimalType == animalType);
                }
                if (Context.Request.Query.ContainsKey("color"))
                {
                    Color color;
                    if(Enum.TryParse(Context.Request.Query["color"], out color))
                        animals = animals.Where(animal => animal.Color == color);
                }
                if (Context.Request.Query.ContainsKey("age_less"))
                {
                    int age;
                    if (Int32.TryParse(Context.Request.Query["age_less"], out age))
                    {
                        DateTime birthDay = DateTime.UtcNow.AddYears(-age);
                        animals = animals.Where(animal => animal.BirthDay !=null &&
                                                          animal.BirthDay.Value >= birthDay);
                    }
                }
                if (Context.Request.Query.ContainsKey("size"))
                {
                    Size size;
                    if (Enum.TryParse(Context.Request.Query["size"], out size))
                        animals = animals.Where(animal => animal.Size == size);
                }
                if (Context.Request.Query.ContainsKey("gender"))
                {
                    Gender gender;
                    if (Enum.TryParse(Context.Request.Query["gender"], out gender))
                        animals = animals.Where(animal => animal.Gender == gender);
                }
                var returns = animals.JoinQueryOver(animal => animal.Shelter)
                                     .Fetch(animal => animal.Shelter).Eager.List();
                return View(returns);
            }
        }

        public IActionResult Shelters()
        {
            ViewBag.PageAction = "Shelters";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var shelters = session.QueryOver<Shelter>();
                return View(shelters.List());
            }            
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
    
    public abstract class GenericAPIController<T>: Controller where T: class
    {
        protected ISessionFactory sessionFactory;
        [HttpGet]
        public IEnumerable<T> GetAll()
        {
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                return session.QueryOver<T>().List();
            }
        }
    }
    
    [Route("api/[controller]")]
    public class ShelterController: GenericAPIController<Shelter>
    {
        public ShelterController(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
    }
    [Route("api/[controller]")]
    public class AnimalController: GenericAPIController<ShelterAnimal>
    {
        public AnimalController(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
    }
}
