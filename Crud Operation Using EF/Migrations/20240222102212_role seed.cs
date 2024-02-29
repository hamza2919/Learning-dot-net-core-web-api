using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud_Operation_Using_EF.Migrations
{
    public partial class roleseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2631d85c-f55f-4a19-ab62-b2d48adb085a", "1", "Admin", "Admin" },
                    { "70eba78e-e509-426c-8a5e-4d8b713b8823", "1", "Manager", "Manager" },
                    { "a922d4ea-c7e6-4fad-bce6-2370e75977ce", "1", "HR", "HR" },
                    { "e2b26ae3-6b4c-4474-ac3a-15d99f77d1aa", "1", "User", "User" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2631d85c-f55f-4a19-ab62-b2d48adb085a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70eba78e-e509-426c-8a5e-4d8b713b8823");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a922d4ea-c7e6-4fad-bce6-2370e75977ce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2b26ae3-6b4c-4474-ac3a-15d99f77d1aa");
        }
    }
}
