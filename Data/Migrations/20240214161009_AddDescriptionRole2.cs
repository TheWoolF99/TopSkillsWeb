using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionRole2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessTypes",
                keyColumn: "TypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccessTypes",
                keyColumn: "TypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccessTypes",
                keyColumn: "TypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccessTypes",
                keyColumn: "TypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccessTypes",
                keyColumn: "TypeId",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessTypes",
                columns: new[] { "TypeId", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "All", "Полный доступ" },
                    { 2, "read", "Просмотр" },
                    { 3, "create", "Добавление" },
                    { 4, "edit", "Редактирование" },
                    { 5, "delete", "Удаление" }
                });
        }
    }
}
