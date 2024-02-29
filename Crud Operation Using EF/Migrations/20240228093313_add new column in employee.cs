using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud_Operation_Using_EF.Migrations
{
    public partial class addnewcolumninemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68f3bfac-f226-4958-9365-6bce49684b68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ebaa270-e0d5-4012-b280-09047e6fe732");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98a9da34-1ab6-404f-a074-ad6f325e73f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be71a521-b3b3-4f64-b971-145d68703e92");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c29fbe1-efef-4106-9419-5c1e3fd003ae", "4", "User", "User" },
                    { "1d516465-5edd-4dfa-b909-62a8608ce845", "2", "Manager", "Manager" },
                    { "dc95df07-528a-4ba0-8147-9d66c8cbcca5", "3", "HR", "HR" },
                    { "f2775f06-814b-46ad-ae9f-50dc2018dd96", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedById",
                table: "Employees",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c29fbe1-efef-4106-9419-5c1e3fd003ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d516465-5edd-4dfa-b909-62a8608ce845");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc95df07-528a-4ba0-8147-9d66c8cbcca5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2775f06-814b-46ad-ae9f-50dc2018dd96");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68f3bfac-f226-4958-9365-6bce49684b68", "3", "HR", "HR" },
                    { "8ebaa270-e0d5-4012-b280-09047e6fe732", "4", "User", "User" },
                    { "98a9da34-1ab6-404f-a074-ad6f325e73f3", "1", "Admin", "Admin" },
                    { "be71a521-b3b3-4f64-b971-145d68703e92", "2", "Manager", "Manager" }
                });
        }
    }
}
