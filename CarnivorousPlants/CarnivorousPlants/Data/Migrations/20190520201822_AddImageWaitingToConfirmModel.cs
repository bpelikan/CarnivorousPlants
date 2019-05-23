using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarnivorousPlants.Data.Migrations
{
    public partial class AddImageWaitingToConfirmModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefaultProjectHistories_MyProjects_MyProjectId",
                table: "DefaultProjectHistories");

            migrationBuilder.DropIndex(
                name: "IX_DefaultProjectHistories_MyProjectId",
                table: "DefaultProjectHistories");

            migrationBuilder.CreateTable(
                name: "ImagesWaitingToConfirm",
                columns: table => new
                {
                    ImageWaitingToConfirmId = table.Column<Guid>(nullable: false),
                    MyTagId = table.Column<Guid>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
                    ProvidedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesWaitingToConfirm", x => x.ImageWaitingToConfirmId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagesWaitingToConfirm");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultProjectHistories_MyProjectId",
                table: "DefaultProjectHistories",
                column: "MyProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultProjectHistories_MyProjects_MyProjectId",
                table: "DefaultProjectHistories",
                column: "MyProjectId",
                principalTable: "MyProjects",
                principalColumn: "MyProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
