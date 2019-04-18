using Microsoft.EntityFrameworkCore.Migrations;

namespace CarnivorousPlants.Data.Migrations
{
    public partial class AddRelationMyProjectToMyTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MyTags_MyProjectId",
                table: "MyTags",
                column: "MyProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTags_MyProjects_MyProjectId",
                table: "MyTags",
                column: "MyProjectId",
                principalTable: "MyProjects",
                principalColumn: "MyProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTags_MyProjects_MyProjectId",
                table: "MyTags");

            migrationBuilder.DropIndex(
                name: "IX_MyTags_MyProjectId",
                table: "MyTags");
        }
    }
}
