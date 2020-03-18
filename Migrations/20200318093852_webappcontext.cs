using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebControlApp.Migrations
{
    public partial class webappcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusEmail",
                columns: table => new
                {
                    StatusEmailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEmail", x => x.StatusEmailId);
                });

            migrationBuilder.CreateTable(
                name: "WebAdress",
                columns: table => new
                {
                    WebAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    IdUser = table.Column<int>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAdress", x => x.WebAddressId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusEmail");

            migrationBuilder.DropTable(
                name: "WebAdress");
        }
    }
}
