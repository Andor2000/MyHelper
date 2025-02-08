using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHelper.Migrations
{
    public partial class AddTableDirectory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DirectoryColomnName",
                table: "Colomns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectoryTableKey",
                table: "Colomns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectoryTableName",
                table: "Colomns",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TableDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rf_Table = table.Column<string>(nullable: true),
                    rf_Colomn = table.Column<string>(nullable: true),
                    Table = table.Column<string>(nullable: true),
                    TabkeKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableDirectories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableDirectories");

            migrationBuilder.DropColumn(
                name: "DirectoryColomnName",
                table: "Colomns");

            migrationBuilder.DropColumn(
                name: "DirectoryTableKey",
                table: "Colomns");

            migrationBuilder.DropColumn(
                name: "DirectoryTableName",
                table: "Colomns");
        }
    }
}
