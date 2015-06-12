using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SweetHome.Models
{
	class DbInitializer
	{
		private ApplicationDbContext _context;
		public DbInitializer(ApplicationDbContext context)
		{
			this._context = context;
		}
		public void InitializeData()
		{
			Console.WriteLine("Starting data initialization");
			InitializeDataAsync().Wait();
			Console.WriteLine("Finished data initialization");
		}
		public async Task InitializeDataAsync()
		{
			var shelters = await CreateShelters();
			await CreateAnimals(shelters);
		}
		private async Task CreateAnimals(IList<Shelter> shelters)
		{
			Console.WriteLine("Creating animals");
			if (_context.ShelterAnimals.Count() == 0)
			{
				Console.WriteLine("Adding new animals");
				IList<ShelterAnimal> _animals = new List<ShelterAnimal>()
				{
					new ShelterAnimal() {
						Name = "keesa",
						BirthDay = new DateTime(2010, 2, 15),
						AnimalType = AnimalType.Cat,
						PlaceType = PlaceType.Shelter,
						Shelter=shelters.Single(s => s.Name == "Хочу Домой")
					},
					new ShelterAnimal() {
						Name = "pyosya",
						BirthDay = new DateTime(2015, 2, 15),
						AnimalType = AnimalType.Dog,
						PlaceType = PlaceType.HomeShelter,
						Color=Color.Dog,
						Size=Size.Medium,
						Shelter=shelters.Single(s => s.Name == "Хочу Домой")
					},
				};
				_context.ShelterAnimals.AddRange(_animals);
				await _context.SaveChangesAsync();
				
			}
			else
			{
				Console.WriteLine("No new animals added");
			}
		}
		private async Task<IList<Shelter>> CreateShelters()
		{
			Console.WriteLine("Creating shelters");
			if (_context.Shelters.Count() == 0)
			{
				Console.WriteLine("Adding new shelters");
				IList<Shelter> _shelters = new List<Shelter>()
				{
					new Shelter() {
						Name = "Хочу Домой",
						Address = "Челябинск",
						Phone = "54321"
					}
				};
				_context.Shelters.AddRange(_shelters);
				await _context.SaveChangesAsync();
				return _shelters;
			}
			else
			{
				Console.WriteLine("No new shelters created");
				return _context.Shelters.ToList();
			}
		}
	}
}