using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModelAbonement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbonimentStart",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Visits",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "Abonements",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    RemainingVisits = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonements", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Abonements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonements");

            migrationBuilder.AddColumn<DateTime>(
                name: "AbonimentStart",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Visits",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
