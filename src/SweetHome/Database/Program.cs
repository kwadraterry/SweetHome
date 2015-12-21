using System;
using Microsoft.Framework.ConfigurationModel;
using System.IO;
using System.Linq;
using SweetHome.Models;
using System.Collections.Generic;

namespace SweetHome
{
    public class Program
    {
        private NHibernate.ISessionFactory sessionFactory;
        public Program()
        {
            this.sessionFactory = DAL.CreateSessionFactory();
        }
        private void ParseShelters(string fileContents)
        {
            var shelters = fileContents.Split(new string[]{"\n\n\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(shelterString => 
                {
                    var shelterFields = shelterString.Split(new char[]{'\n'});
                    return new Shelter
                    {
                        Name = shelterFields[0],
                        Address = shelterFields[1],
                        PhoneNumbers = shelterFields[2].Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries),
						Info = shelterFields[3],
						VKGroup = shelterFields[4],
						Image = shelterFields[5],
                        URL = shelterFields[6]
                    };
                });
            Console.WriteLine("Finished parsing file");
            Console.WriteLine("Saving entries to the database...");
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                foreach(var shelter in shelters)
                {
                    session.Save(shelter);
                }
                transaction.Commit();
            }
            Console.WriteLine("Finished saving items to the database");
        }
        private void ParseAnimals(string fileContents)
        {
            Shelter shelter;
          
            var animals = fileContents.Split(new string[]{"\n\n\n\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(animalString =>
                {
                    var animalFields = animalString.Split(new string[]{"\n\n"}, StringSplitOptions.RemoveEmptyEntries);
                    
                    DateTime? birthday = null;
                    using (var session = sessionFactory.OpenSession())
                    using (session.BeginTransaction())
                    {
                        shelter = session.QueryOver<Shelter>().Where(sh => sh.Id == Int32.Parse(animalFields[0])).Take(1).List()[0];
                        
                    }
                    int months;
                    
                    if (Int32.TryParse(animalFields[3], out months))
                    {
                        birthday = DateTime.UtcNow.AddMonths(- months);
                    }
                    string type = animalFields[1];
                    AnimalType animalType;
                    if (type == "0") {
                        animalType = AnimalType.Dog;
                    } else {
                        animalType = AnimalType.Cat;
                    }   
                    
                    Gender gender;
                    //  if (animalFields[1] == "М")
                    //  {
                    //      gender = Gender.Male;
                    //  }
                    //  else if (animalFields[1] == "Ж")
                    //  {
                    //      gender = Gender.Female;
                    //  }
                    //  else
                    //  {
                    gender = Gender.Unknown;
                    //  }
                    List<String> Images = new List<String>(animalFields[5].Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries));
                    Images.RemoveAt(Images.Count-1);
                    return new ShelterAnimal
                    {
                        Name = animalFields[2].ToLower(),
                        AnimalType = animalType,
                        BirthDay = birthday,
                        Gender = gender,
                        Info = animalFields[4],
                        Images = Images,
                        Created = DateTime.UtcNow,
                        Shelter = shelter
                    };
                });
            Console.WriteLine("Finished parsing file");
            Console.WriteLine("Saving entries to the database...");
            foreach(var animal in animals)
            {
                using (var session = sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(animal);
                        transaction.Commit();
                        Console.WriteLine("Записали " + animal.Name);
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("violates unique constraint");
                    }
                }
            }
            Console.WriteLine("Finished saving items to the database");
        }
        public void Main(string[] args)
        {
            Console.WriteLine("Started");
            Console.WriteLine("Reading configuration...");
            var config = new Configuration()
                            .AddCommandLine(args);
            Console.WriteLine("Finished reading configuration");

            string filename = config.Get("file");
            string type = config.Get("type");
            
            string line = null;
            Console.WriteLine("Reading file...");
            using (StreamReader sr = new StreamReader(filename))
                line = sr.ReadToEnd();
            Console.WriteLine("Finished reading file");
            Console.WriteLine("Parsing file...");
            string command = config.Get("command");
            if (command == "animals")
            {
                ParseAnimals(line);

            }
            else if (command == "shelters")
            {
                ParseShelters(line);
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
}
