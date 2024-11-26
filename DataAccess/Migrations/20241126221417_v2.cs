using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6f93316a-799e-45e4-b0cc-7d1dda8ce74b", "e84c604e-3cdd-4e65-8089-97919d768142" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "64d29656-6473-4bca-8864-ba9664a8fea7", "54b259d9-595a-4fbf-a972-c5577b78c8e7" });
        }
    }
}
