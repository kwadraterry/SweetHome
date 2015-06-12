using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace SweetHome.Migrations
{
    public partial class ShelterAnimalChange0 : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "BirthDay",
                table: "ShelterAnimal",
                type: "datetime2",
                nullable: false);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "BirthDay",
                table: "ShelterAnimal",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
