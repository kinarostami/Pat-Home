using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class initDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 935, DateTimeKind.Local).AddTicks(2935));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 937, DateTimeKind.Local).AddTicks(7967));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 937, DateTimeKind.Local).AddTicks(8004));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 937, DateTimeKind.Local).AddTicks(8009));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 937, DateTimeKind.Local).AddTicks(8012));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 42, 29, 937, DateTimeKind.Local).AddTicks(8026));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 232, DateTimeKind.Local).AddTicks(2095));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 313, DateTimeKind.Local).AddTicks(6110));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 313, DateTimeKind.Local).AddTicks(6143));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 313, DateTimeKind.Local).AddTicks(6147));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 313, DateTimeKind.Local).AddTicks(6149));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ShippingCosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 8, 21, 30, 44, 313, DateTimeKind.Local).AddTicks(6163));
        }
    }
}
