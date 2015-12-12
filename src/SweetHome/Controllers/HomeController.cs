using System;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
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
        private readonly Random rng;
        public HomeController(NHibernate.ISessionFactory sessionFactory, IHostingEnvironment hostingEnvironment)
        {
            this.sessionFactory = sessionFactory;
            this.hostingEnvironment = hostingEnvironment;
            this.rng = new Random();
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
                var shelters = session.QueryOver<Shelter>();
                shelters = shelters.Where(shelter => shelter.Id != 1);
                ViewBag.Shelters = shelters.List();
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
        
        private string RemoveExtraText(string value)
        {
            var allowedChars = "01234567890,";
           
            return new string(value.Replace("+7","8").Where(c => allowedChars.Contains(c)).ToArray());
        }
        
        [HttpPost]
        public IActionResult AddAnimal(string animalName,
            string animalType, string info, string images,
            string shelterId, string ownerName, string phoneNumbers)
        {
            var shelterIdNum = 1;
            if (shelterId != null){
                shelterIdNum = Int32.Parse(shelterId);
            }
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
            phoneNumbers = RemoveExtraText(phoneNumbers);
            Console.WriteLine(phoneNumbers);
            using(var session = sessionFactory.OpenSession())
            using(var transaction = session.BeginTransaction())
            {
                var shelter = session.QueryOver<Shelter>().List().Where(s => s.Id == shelterIdNum).First();
                var imagesList = images.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
                var animal = new ShelterAnimal
                {
                    Name = animalName,
                    AnimalType = animalTypeEnum,
                    OwnerName = ownerName,
                    Info = info,
                    Images = imagesList,
                    PhoneNumbers = phoneNumbers.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries),
                    Created = DateTime.UtcNow,
                    Shelter = shelter
                };
                
                session.Save(animal);
                transaction.Commit();
            }
            return RedirectToAction("New");
        }
        
        [HttpPost]
        public FileUploadInfo ImageUpload(IFormFile file)
        {
            var dirtyFileName = new ContentDisposition(file.ContentDisposition).FileName;
            dirtyFileName = Regex.Replace(dirtyFileName, "[@,\\\";'\\\\]", string.Empty);
            var randomString = Convert.ToBase64String(BitConverter.GetBytes(this.rng.Next()));
            var fileName = Path.Combine(
                "media",
                randomString + dirtyFileName);
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
