using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Infrastructure.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ent");

            migrationBuilder.CreateTable(
                name: "EntityLogDbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditorID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLogDbSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Selection",
                schema: "Ent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SelectionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditorID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ent_Selection", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLogDbSet");

            migrationBuilder.DropTable(
                name: "Selection",
                schema: "Ent");
        }
    }
}
