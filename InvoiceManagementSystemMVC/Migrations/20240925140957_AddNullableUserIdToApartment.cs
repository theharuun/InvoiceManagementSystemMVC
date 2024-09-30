using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagementSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableUserIdToApartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_UserAdmin",
                table: "Apartments");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_UserAdmin",
                table: "Apartments",
                column: "UserId",
                principalTable: "UserAdmins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_UserAdmin",
                table: "Apartments");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_UserAdmin",
                table: "Apartments",
                column: "UserId",
                principalTable: "UserAdmins",
                principalColumn: "UserId");
        }
    }
}
