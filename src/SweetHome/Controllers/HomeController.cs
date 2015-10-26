using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using System.Linq;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;

namespace SweetHome.Controllers
{
    public class FileUploadInfo
    {
        public String FileName { get; set; }
    } 
    public class HomeController : Controller
    {   
        private readonly NHibernate.ISessionFactory sessionFactory;
        private readonly IHostingEnvironment hostingEnvironment;
        public HomeController(NHibernate.ISessionFactory sessionFactory, IHostingEnvironment hostingEnvironment)
        {
            this.sessionFactory = sessionFactory;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.PageAction = "Index";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var dogs = session.QueryOver<ShelterAnimal>()
                                  .Fetch(animal => animal.Shelter).Eager
                                  .Where(animal => animal.AnimalType == AnimalType.Dog).List();
                var cats = session.QueryOver<ShelterAnimal>()
                                  .Fetch(animal => animal.Shelter).Eager
                                  .Where(animal => animal.AnimalType == AnimalType.Cat).List();
                Random rand = new Random();
                ViewBag.Cats = cats.OrderBy(x => rand.Next()).Take(2);
                ViewBag.Dogs = dogs.OrderBy(x => rand.Next()).Take(2);
                return View();
            }
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
                ViewBag.Type = null;
                ViewBag.Color = null;
                ViewBag.AgeLess = null;
                ViewBag.Size = null;
                ViewBag.Gender = null;
                ViewBag.All = true;
                var shelters = session.QueryOver<Shelter>().List();
                ViewBag.Shelters = shelters;
                var animals = session.QueryOver<ShelterAnimal>();
                if (Context.Request.Query.ContainsKey("shelter"))
                {
                    int id;
                    if (Int32.TryParse(Context.Request.Query["shelter"], out id))
                    {
                        ViewBag.ShelterId = id;
                        ViewBag.All = false;
                        animals = animals.Where(animal => animal.Shelter.Id == id);
                    }
                }
                if (Context.Request.Query.ContainsKey("type"))
                {
                    AnimalType animalType;
                    if (Enum.TryParse(Context.Request.Query["type"], out animalType))
                    {
                        ViewBag.Type = animalType;
                        ViewBag.All = false;
                        animals = animals.Where(animal => animal.AnimalType == animalType);
                    }
                }
                if (Context.Request.Query.ContainsKey("color"))
                {
                    Color color;
                    if(Enum.TryParse(Context.Request.Query["color"], out color))
                    {
                        ViewBag.Color = color;
                        ViewBag.All = false;
                        animals = animals.Where(animal => animal.Color == color);
                    }
                }
                if (Context.Request.Query.ContainsKey("age_less"))
                {
                    int age;
                    if (Int32.TryParse(Context.Request.Query["age_less"], out age))
                    {
                        ViewBag.AgeLess = age;
                        ViewBag.All = false;
                        DateTime birthDay = DateTime.UtcNow.AddYears(-age);
                        animals = animals.Where(animal => animal.BirthDay !=null &&
                                                          animal.BirthDay.Value >= birthDay);
                    }
                }
                if (Context.Request.Query.ContainsKey("size"))
                {
                    Size size;
                    if (Enum.TryParse(Context.Request.Query["size"], out size))
                    {
                        ViewBag.Size = size;
                        ViewBag.All = false;
                        animals = animals.Where(animal => animal.Size == size);
                    }
                }
                if (Context.Request.Query.ContainsKey("gender"))
                {
                    Gender gender;
                    if (Enum.TryParse(Context.Request.Query["gender"], out gender))
                    {
                        ViewBag.Gender = gender;
                        ViewBag.All = false;
                        animals = animals.Where(animal => animal.Gender == gender);
                    }
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


        public IActionResult New()
        {
            ViewBag.PageAction = "New";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var shelters = session.QueryOver<Shelter>().List();
                ViewBag.Shelters = shelters;
                return View();
            }
        }
        
        [HttpPost]
        public IActionResult AddAnimal(string animalName, string animalType, string info, string images, string shelterId)
        {
            var shelterIdNum = Int32.Parse(shelterId);
            AnimalType animalTypeEnum;
            Enum.TryParse(animalType, out animalTypeEnum);
            if (animalName == null) {
                animalName = "";
            }
            if (info == null) {
                info = "";
            }
            if (images == null) {
                images = "";
            }
            using(var session = sessionFactory.OpenSession())
            using(var transaction = session.BeginTransaction())
            {
                var shelter = session.QueryOver<Shelter>().List().Where(s => s.Id == shelterIdNum).First();
                var animal = new ShelterAnimal
                {
                    Name = animalName,
                    AnimalType = animalTypeEnum,
                    Info = info,
                    Images = images.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries),
                    Created = DateTime.UtcNow,
                    Shelter = shelter
                };
                session.Save(animal);
                transaction.Commit();
            }
            return New();
        }
        
        [HttpPost]
        public FileUploadInfo ImageUpload(IFormFile file)
        {
            var fileName = Path.Combine(
                "media",
                Guid.NewGuid().ToString() +".png");
            file.SaveAs(Path.Combine(
                hostingEnvironment.WebRootPath,
                fileName));
            return new FileUploadInfo { FileName = "/" + fileName };
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
