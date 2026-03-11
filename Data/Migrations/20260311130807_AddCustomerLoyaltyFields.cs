using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerLoyaltyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStayDate",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoyaltyLevel",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Customers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalStays",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStayDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LoyaltyLevel",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TotalStays",
                table: "Customers");
        }
    }
}
