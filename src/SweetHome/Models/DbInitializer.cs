using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SweetHome.Models
{
	class DbInitializer
	{
		private ApplicationDbContext _context;
		private static IList<Shelter> _shelters = new List<Shelter>()
		{
			new Shelter() {
				Name = "Хочу Домой",
				Address = "Челябинск",
				Phone = "54321"
			}
		};
		private static IEnumerable<ShelterAnimal> _animals = new List<ShelterAnimal>()
		{
			new ShelterAnimal() {
				Name = "keesa",
				BirthDay = new DateTime(2010, 2, 15),
				AnimalType = AnimalType.Cat,
				PlaceType = PlaceType.Shelter
			},
			new ShelterAnimal() {
				Name = "pyosya",
				BirthDay = new DateTime(2015, 2, 15),
				AnimalType = AnimalType.Dog,
				PlaceType = PlaceType.HomeShelter,
				Color=Color.Dog,
				Size=Size.Medium
			},
		};
		public DbInitializer(ApplicationDbContext context)
		{
			this._context = context;
		}
		public void InitializeData()
		{
			System.Console.WriteLine("Initializing data");
			System.Console.WriteLine("Adding rows");
			CreateShelters().Wait();
			CreateAnimals().Wait();
		}
		private async Task CreateAnimals()
		{
			if (_context.ShelterAnimals.Count() == 0)
			{
				_context.ShelterAnimals.AddRange(_animals);
				await _context.SaveChangesAsync();
			}
		}
		private async Task CreateShelters()
		{
			if (_context.Shelters.Count() == 0)
			{
				_context.Shelters.AddRange(_shelters);
				await _context.SaveChangesAsync();
			}
		}
	}
}