using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Framework.OptionsModel;

namespace SweetHome.Models
{
    //  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //  {
    //      private static bool _created = false;
    //      public DbSet<Shelter> Shelters { get; set; }
    //      public DbSet<ShelterAnimal> ShelterAnimals { get; set; }
        
    //      public ApplicationDbContext()
    //      {            
    //          // Create the database and schema if it doesn't exist
    //          // This is a temporary workaround to create database until Entity Framework database migrations 
    //          // are supported in ASP.NET 5
            
    //      }

    //      protected override void OnModelCreating(ModelBuilder builder)
    //      {
    //          builder.Entity<Shelter>().Key(s => s.ShelterId);
            
    //          base.OnModelCreating(builder);
    //      }
    //  }
}