using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceManagementSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApartmentTypes",
                columns: table => new
                {
                    ATypeId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApartmentTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentTypes", x => x.ATypeId);
                });

            migrationBuilder.CreateTable(
                name: "BillTypes",
                columns: table => new
                {
                    BTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillTypes", x => x.BTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserAdmins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TcNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentId = table.Column<int>(type: "int", nullable: true),
                    Role = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdmins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApartmentTypeId = table.Column<short>(type: "smallint", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartment_ApartmentType",
                        column: x => x.ApartmentTypeId,
                        principalTable: "ApartmentTypes",
                        principalColumn: "ATypeId");
                    table.ForeignKey(
                        name: "FK_Apartment_UserAdmin",
                        column: x => x.UserId,
                        principalTable: "UserAdmins",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillType = table.Column<int>(type: "int", nullable: true),
                    BillPayment = table.Column<long>(type: "bigint", nullable: true),
                    ApartmentId = table.Column<int>(type: "int", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bill_Apartment",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentId");
                    table.ForeignKey(
                        name: "FK_Bill_BillType",
                        column: x => x.BillType,
                        principalTable: "BillTypes",
                        principalColumn: "BTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ApartmentTypeId",
                table: "Apartments",
                column: "ApartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ApartmentId",
                table: "Bills",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillType",
                table: "Bills",
                column: "BillType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "BillTypes");

            migrationBuilder.DropTable(
                name: "ApartmentTypes");

            migrationBuilder.DropTable(
                name: "UserAdmins");
        }
    }
}
