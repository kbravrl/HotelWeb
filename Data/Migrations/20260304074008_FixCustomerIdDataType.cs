using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCustomerIdDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerId1",
                table: "Reservations");

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Reservations");

            // Mevcut rezervasyonları sil (veri uyumsuzluğu nedeniyle)
            migrationBuilder.Sql(@"DELETE FROM ""Reservations"";");

            // PostgreSQL için önce default değeri kaldır, sonra tip dönüşümü yap
            migrationBuilder.Sql(@"
                ALTER TABLE ""Reservations"" 
                ALTER COLUMN ""CustomerId"" DROP DEFAULT;

                ALTER TABLE ""Reservations""
                ALTER COLUMN ""CustomerId"" TYPE integer
                USING CASE 
                    WHEN ""CustomerId"" ~ '^\d+$' THEN ""CustomerId""::integer 
                    ELSE 0 
                END;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations");

            // PostgreSQL için USING clause ile tip dönüşümü (geri alma)
            migrationBuilder.Sql(@"
                ALTER TABLE ""Reservations""
                ALTER COLUMN ""CustomerId"" TYPE text
                USING ""CustomerId""::text;
            ");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Reservations",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CheckIn", "CheckOut", "CreatedAt", "CustomerId", "CustomerId1", "GuestCount", "RoomId", "Status", "TotalPrice" },
                values: new object[] { 1, new DateOnly(2026, 2, 26), new DateOnly(2026, 3, 1), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), "2", null, 2, 4, 1, 0m });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId1",
                table: "Reservations",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId1",
                table: "Reservations",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
