﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                var shelters = session.QueryOver<Shelter>().List();
                ViewBag.Shelters = shelters;
                var animals = session.QueryOver<ShelterAnimal>();
                if (Context.Request.Query.ContainsKey("shelter"))
                {
                    int id;
                    if (Int32.TryParse(Context.Request.Query["shelter"], out id))
                        animals = animals.Where(animal => animal.Shelter.Id == id);
                }
                if (Context.Request.Query.ContainsKey("type"))
                {
                    AnimalType animalType;
                    if (Enum.TryParse(Context.Request.Query["type"], out animalType))
                    {
                        ViewBag.Type = animalType;
                        animals = animals.Where(animal => animal.AnimalType == animalType);
                    }
                }
                if (Context.Request.Query.ContainsKey("color"))
                {
                    Color color;
                    if(Enum.TryParse(Context.Request.Query["color"], out color))
                    {
                        ViewBag.Color = color;
                        animals = animals.Where(animal => animal.Color == color);
                    }
                }
                if (Context.Request.Query.ContainsKey("age_less"))
                {
                    int age;
                    if (Int32.TryParse(Context.Request.Query["age_less"], out age))
                    {
                        ViewBag.AgeLess = age;
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
                        animals = animals.Where(animal => animal.Size == size);
                    }
                }
                if (Context.Request.Query.ContainsKey("gender"))
                {
                    Gender gender;
                    if (Enum.TryParse(Context.Request.Query["gender"], out gender))
                    {
                        ViewBag.Gender = gender;
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
