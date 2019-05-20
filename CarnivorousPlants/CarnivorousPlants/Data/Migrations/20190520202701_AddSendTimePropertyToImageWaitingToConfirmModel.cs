using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarnivorousPlants.Data.Migrations
{
    public partial class AddSendTimePropertyToImageWaitingToConfirmModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendTime",
                table: "ImagesWaitingToConfirm",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendTime",
                table: "ImagesWaitingToConfirm");
        }
    }
}
