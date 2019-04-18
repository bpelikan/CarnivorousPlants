using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarnivorousPlants.Data.Migrations
{
    public partial class AddMyTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyTags",
                columns: table => new
                {
                    MyTagId = table.Column<Guid>(nullable: false),
                    MyProjectId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTags", x => x.MyTagId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyTags");
        }
    }
}
