using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHelper.Migrations
{
    public partial class AddDirectoryTableNickname2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableNickname",
                table: "TableDirectories",
                maxLength: 70,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableNickname",
                table: "TableDirectories");
        }
    }
}
