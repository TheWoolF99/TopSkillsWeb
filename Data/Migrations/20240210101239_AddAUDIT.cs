using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAUDIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    AuditEntryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntitySetName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.AuditEntryID);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryProperties",
                columns: table => new
                {
                    AuditEntryPropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditEntryID = table.Column<int>(type: "int", nullable: false),
                    RelationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryProperties", x => x.AuditEntryPropertyID);
                    table.ForeignKey(
                        name: "FK_AuditEntryProperties_AuditEntries_AuditEntryID",
                        column: x => x.AuditEntryID,
                        principalTable: "AuditEntries",
                        principalColumn: "AuditEntryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryProperties_AuditEntryID",
                table: "AuditEntryProperties",
                column: "AuditEntryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntryProperties");

            migrationBuilder.DropTable(
                name: "AuditEntries");
        }
    }
}
