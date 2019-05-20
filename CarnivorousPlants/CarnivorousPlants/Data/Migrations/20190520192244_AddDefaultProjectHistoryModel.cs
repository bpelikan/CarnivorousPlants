using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarnivorousPlants.Data.Migrations
{
    public partial class AddDefaultProjectHistoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultProjectHistories",
                columns: table => new
                {
                    DefaultProjectHistoryId = table.Column<Guid>(nullable: false),
                    MyProjectId = table.Column<Guid>(nullable: false),
                    SettedBy = table.Column<string>(nullable: true),
                    SettingTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultProjectHistories", x => x.DefaultProjectHistoryId);
                    table.ForeignKey(
                        name: "FK_DefaultProjectHistories_MyProjects_MyProjectId",
                        column: x => x.MyProjectId,
                        principalTable: "MyProjects",
                        principalColumn: "MyProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultProjectHistories_MyProjectId",
                table: "DefaultProjectHistories",
                column: "MyProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultProjectHistories");
        }
    }
}
