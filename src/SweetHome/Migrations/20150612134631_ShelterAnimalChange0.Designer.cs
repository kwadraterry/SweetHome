using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using SweetHome.Models;

namespace SweetHome.Migrations
{
    [ContextType(typeof(ApplicationDbContext))]
    partial class ShelterAnimalChange0
    {
        public override string Id
        {
            get { return "20150612134631_ShelterAnimalChange0"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta4-12943"; }
        }
        
        public override IModel Target
        {
            get
            {
                var builder = new BasicModelBuilder()
                    .Annotation("SqlServer:ValueGeneration", "Identity");
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                    {
                        b.Property<string>("ConcurrencyStamp")
                            .ConcurrencyToken()
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("NormalizedName")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                        b.Annotation("Relational:TableName", "AspNetRoles");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.Property<string>("ClaimType")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("ClaimValue")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 2)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("RoleId")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                        b.Annotation("Relational:TableName", "AspNetRoleClaims");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.Property<string>("ClaimType")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("ClaimValue")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 2)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("UserId")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                        b.Annotation("Relational:TableName", "AspNetUserClaims");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.Property<string>("LoginProvider")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("ProviderDisplayName")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("ProviderKey")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("UserId")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("LoginProvider", "ProviderKey");
                        b.Annotation("Relational:TableName", "AspNetUserLogins");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.Property<string>("RoleId")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("UserId")
                            .Annotation("OriginalValueIndex", 1);
                        b.Key("UserId", "RoleId");
                        b.Annotation("Relational:TableName", "AspNetUserRoles");
                    });
                
                builder.Entity("SweetHome.Models.ApplicationUser", b =>
                    {
                        b.Property<int>("AccessFailedCount")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("ConcurrencyStamp")
                            .ConcurrencyToken()
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("Email")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<bool>("EmailConfirmed")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<string>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 4);
                        b.Property<bool>("LockoutEnabled")
                            .Annotation("OriginalValueIndex", 5);
                        b.Property<DateTimeOffset?>("LockoutEnd")
                            .Annotation("OriginalValueIndex", 6);
                        b.Property<string>("NormalizedEmail")
                            .Annotation("OriginalValueIndex", 7);
                        b.Property<string>("NormalizedUserName")
                            .Annotation("OriginalValueIndex", 8);
                        b.Property<string>("PasswordHash")
                            .Annotation("OriginalValueIndex", 9);
                        b.Property<string>("PhoneNumber")
                            .Annotation("OriginalValueIndex", 10);
                        b.Property<bool>("PhoneNumberConfirmed")
                            .Annotation("OriginalValueIndex", 11);
                        b.Property<string>("SecurityStamp")
                            .Annotation("OriginalValueIndex", 12);
                        b.Property<bool>("TwoFactorEnabled")
                            .Annotation("OriginalValueIndex", 13);
                        b.Property<string>("UserName")
                            .Annotation("OriginalValueIndex", 14);
                        b.Key("Id");
                        b.Annotation("Relational:TableName", "AspNetUsers");
                    });
                
                builder.Entity("SweetHome.Models.Shelter", b =>
                    {
                        b.Property<string>("Address")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("Phone")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<int>("ShelterId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 3)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Key("ShelterId");
                    });
                
                builder.Entity("SweetHome.Models.ShelterAnimal", b =>
                    {
                        b.Property<int>("AnimalType")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<DateTime>("BirthDay")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("Color")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<DateTime>("Created")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<string>("ImagesSerialized")
                            .Annotation("OriginalValueIndex", 4);
                        b.Property<string>("Info")
                            .Annotation("OriginalValueIndex", 5);
                        b.Property<bool>("IsForFlat")
                            .Annotation("OriginalValueIndex", 6);
                        b.Property<bool>("IsForHome")
                            .Annotation("OriginalValueIndex", 7);
                        b.Property<bool>("IsHappy")
                            .Annotation("OriginalValueIndex", 8);
                        b.Property<bool>("IsHealth")
                            .Annotation("OriginalValueIndex", 9);
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 10);
                        b.Property<int>("PlaceType")
                            .Annotation("OriginalValueIndex", 11);
                        b.Property<int>("ShelterAnimalId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 12)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<int>("ShelterId")
                            .Annotation("OriginalValueIndex", 13);
                        b.Property<int>("Size")
                            .Annotation("OriginalValueIndex", 14);
                        b.Property<bool>("Toilet")
                            .Annotation("OriginalValueIndex", 15);
                        b.Key("ShelterAnimalId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.ForeignKey("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", "RoleId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.ForeignKey("SweetHome.Models.ApplicationUser", "UserId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.ForeignKey("SweetHome.Models.ApplicationUser", "UserId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", b =>
                    {
                        b.ForeignKey("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", "RoleId");
                        b.ForeignKey("SweetHome.Models.ApplicationUser", "UserId");
                    });
                
                builder.Entity("SweetHome.Models.ShelterAnimal", b =>
                    {
                        b.ForeignKey("SweetHome.Models.Shelter", "ShelterId");
                    });
                
                return builder.Model;
            }
        }
    }
}
