using System;
using Microsoft.Framework.ConfigurationModel;
using System.IO;
using System.Linq;
using SweetHome.Models;

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
                        Phone = shelterFields[2],
						Info = shelterFields[3],
						VKGroup = shelterFields[4],
						Image = shelterFields[5]
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
        private void ParseAnimals(string fileContents, int shelterId)
        {
            Shelter shelter;
            using (var session = sessionFactory.OpenSession())
            using (session.BeginTransaction())
            {
                shelter = session.QueryOver<Shelter>().Where(sh => sh.Id == shelterId).Take(1).List()[0];
            }
            var animals = fileContents.Split(new string[]{"\n\n\n\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(animalString =>
                {
                    var animalFields = animalString.Split(new string[]{"\n\n"}, StringSplitOptions.RemoveEmptyEntries);
                    
                    DateTime? birthday = null;
                    int years;
                    if (Int32.TryParse(animalFields[2], out years))
                    {
                        birthday = DateTime.UtcNow.AddYears(- years);
                    }
                    
                    Gender gender;
                    if (animalFields[1] == "М")
                    {
                        gender = Gender.Male;
                    }
                    else if (animalFields[1] == "Ж")
                    {
                        gender = Gender.Female;
                    }
                    else
                    {
                        gender = Gender.Unknown;
                    }
                    
                    return new ShelterAnimal
                    {
                        Name = animalFields[0],
                        AnimalType = AnimalType.Dog,
                        BirthDay = birthday,
                        Gender = gender,
                        Info = animalFields[3],
                        Images = animalFields[4].Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries),
                        Created = DateTime.UtcNow,
                        Shelter = shelter
                    };
                });
            Console.WriteLine("Finished parsing file");
            Console.WriteLine("Saving entries to the database...");
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                foreach(var animal in animals)
                {
                    session.Save(animal);
                }
                transaction.Commit();
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
            string line = null;
            Console.WriteLine("Reading file...");
            using (StreamReader sr = new StreamReader(filename))
                line = sr.ReadToEnd();
            Console.WriteLine("Finished reading file");
            Console.WriteLine("Parsing file...");
            string command = config.Get("command");
            if (command == "animals")
            {
                int shelterId = Int32.Parse(config.Get("shelter_id"));
				Console.WriteLine (shelterId);
                ParseAnimals(line, shelterId);

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
