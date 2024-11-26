using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4b11c41b-27bc-410b-bd42-c81bea0898d8", "38fccbe5-9650-458f-8a69-d700aa8ff002" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ce0774bc-fb2a-40bc-84ac-fe57f557c25d", "0b3c66c1-4dc7-4a9c-aac1-3bcb34f405d7" });
        }
    }
}
