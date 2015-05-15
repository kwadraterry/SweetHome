using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace SweetHome.Migrations
{
    public partial class ShelterAnimalImages : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "Created",
                table: "ShelterAnimal",
                type: "datetime2",
                nullable: false);
            migration.AddColumn(
                name: "ImagesSerialized",
                table: "ShelterAnimal",
                type: "nvarchar(max)",
                nullable: true);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropColumn(name: "ImagesSerialized", table: "ShelterAnimal");
            migration.AlterColumn(
                name: "Created",
                table: "ShelterAnimal",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
