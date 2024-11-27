using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "131a577b-56dd-47fe-9cfb-ccdb14815705", "bd961a31-57f6-44a5-b815-5fc224202632" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b4c163ee-7fe9-49c7-bac1-69d5db322dc7", "45f58f69-a46f-442d-b425-9dc25e9f4149" });
        }
    }
}
