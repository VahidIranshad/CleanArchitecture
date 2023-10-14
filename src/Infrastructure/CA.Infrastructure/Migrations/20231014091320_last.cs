using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA.Infrastructure.Migrations
{
    public partial class last : Migration
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

            migrationBuilder.CreateTable(
                name: "TValue",
                schema: "Ent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditorID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ent_TValue", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Ent",
                table: "TValue",
                columns: new[] { "Id", "CreateDate", "CreatorID", "LastEditDate", "LastEditorID", "Title" },
                values: new object[] { 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8e445865-a24d-4543-a6c6-9443d048cdb9", new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8e445865-a24d-4543-a6c6-9443d048cdb9", "Test" });

            migrationBuilder.CreateIndex(
                name: "IX_Selection_SelectionType_Title",
                schema: "Ent",
                table: "Selection",
                columns: new[] { "SelectionType", "Title" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLogDbSet");

            migrationBuilder.DropTable(
                name: "Selection",
                schema: "Ent");

            migrationBuilder.DropTable(
                name: "TValue",
                schema: "Ent");
        }
    }
}
