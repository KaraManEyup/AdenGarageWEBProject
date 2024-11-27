using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e16e522a-134b-4a7f-81e2-6b950e2924f5", "1f01fac1-a3e4-4203-a244-78ecf884a5c0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "73c52485-bdc8-41e3-b79f-05463a64d079", "3a2c615b-3c7a-4cfb-9feb-c6d3563d93a6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5b8a0e4b-0815-4d2c-88f6-f2563dee1911", "f4eca1f2-53cc-4ffd-af82-1bb70e433a20" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f9d6d0dc-7ee3-4290-95d8-075f25fe05d6", "502ddf7d-5db4-4202-aa9f-98c7854a10bd" });
        }
    }
}
