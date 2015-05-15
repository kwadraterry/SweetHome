using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SweetHome.Models
{
	class DbInitializer
	{
		private ApplicationDbContext _context;
		private IEnumerable<ShelterAnimal> _animals = new List<ShelterAnimal>()
		{
			new ShelterAnimal() {Name = "keesa", BirthDay = new DateTime(2010, 2, 15)},
			new ShelterAnimal() {Name = "pyosya"}
		};
		public DbInitializer(ApplicationDbContext context)
		{
			this._context = context;
		}
		public void InitializeData()
		{
			if (_context.Database.EnsureCreated())
			{
				CreateAnimals().Wait();
			}
		}
		private async Task CreateAnimals()
		{
			if (_context.ShelterAnimals.Count() == 0)
			{
				_context.ShelterAnimals.AddRange(_animals);
				await _context.SaveChangesAsync();
			}
		}
	}
}