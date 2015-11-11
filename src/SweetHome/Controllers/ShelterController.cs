using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using SweetHome.Models;
using NHibernate;

namespace SweetHome.Controllers
{
    public class SheltersController : Controller
    {   
        private readonly ISessionFactory sessionFactory;
        public SheltersController(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
        public IActionResult HochuDomoi()
        {
            ViewBag.PageAction = "Shelters";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var shelter = session.QueryOver<Shelter>().Take(1).List()[0];
			    return View(shelter);
            }
            
        }

        public IActionResult LuchikNadejdy()
        {
            ViewBag.PageAction = "Shelters";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var shelter = session.QueryOver<Shelter>().Take(1).List()[0];
			    return View(shelter);
            }
        }
        
        public IActionResult Drug()
        {
            ViewBag.PageAction = "Shelters";
            using(var session = sessionFactory.OpenSession())
            using(session.BeginTransaction())
            {
                var shelter = session.QueryOver<Shelter>().Take(1).List()[0];
			    return View(shelter);
            }
        }
    }
       
}
