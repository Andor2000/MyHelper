using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHelper.Migrations
{
    public partial class AddIsdeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tables",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Colomns",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Colomns");
        }
    }
}
