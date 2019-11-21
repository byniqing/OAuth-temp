using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Info.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BindId = table.Column<int>(nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    PassWord = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(100)", nullable: true, defaultValue: "local"),
                    Created = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 11, 19, 11, 14, 25, 67, DateTimeKind.Local).AddTicks(9389))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
