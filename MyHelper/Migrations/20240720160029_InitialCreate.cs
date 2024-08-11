using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHelper.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsTemplateScript = table.Column<bool>(nullable: false),
                    Guid = table.Column<string>(nullable: true),
                    Sprint = table.Column<string>(nullable: true),
                    Task = table.Column<string>(nullable: true),
                    Project = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colomns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Records = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsEqualsRecordStar = table.Column<bool>(nullable: false),
                    IsQuotes = table.Column<bool>(nullable: false),
                    TableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colomns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colomns_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colomns_TableId",
                table: "Colomns",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colomns");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
