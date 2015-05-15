using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace SweetHome.Migrations
{
    public partial class AnimalShelter : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "ShelterAnimal",
                columns: table => new
                {
                    AnimalType = table.Column(type: "int", nullable: false),
                    BirthDay = table.Column(type: "nvarchar(max)", nullable: true),
                    Color = table.Column(type: "int", nullable: false),
                    Created = table.Column(type: "nvarchar(max)", nullable: true),
                    Info = table.Column(type: "nvarchar(max)", nullable: true),
                    IsForFlat = table.Column(type: "bit", nullable: false),
                    IsForHome = table.Column(type: "bit", nullable: false),
                    IsHappy = table.Column(type: "bit", nullable: false),
                    IsHealth = table.Column(type: "bit", nullable: false),
                    Name = table.Column(type: "nvarchar(max)", nullable: true),
                    PlaceType = table.Column(type: "int", nullable: false),
                    ShelterAnimalId = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    ShelterId = table.Column(type: "int", nullable: false),
                    Size = table.Column(type: "int", nullable: false),
                    Toilet = table.Column(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelterAnimal", x => x.ShelterAnimalId);
                    table.ForeignKey(
                        name: "FK_ShelterAnimal_Shelter_ShelterId",
                        columns: x => x.ShelterId,
                        referencedTable: "Shelter",
                        referencedColumn: "ShelterId");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("ShelterAnimal");
        }
    }
}
