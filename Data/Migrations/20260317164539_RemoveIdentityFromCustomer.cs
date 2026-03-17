using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
