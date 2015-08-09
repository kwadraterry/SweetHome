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
            var animals = line.Split(new string[]{"\n\n\n\n"}, StringSplitOptions.RemoveEmptyEntries)
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
                        Created = DateTime.UtcNow
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
    }
}
