using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "34a2f5c9-765d-4f49-92b5-b23a43173eb0", "f4224ce4-1db5-44e4-a7c0-f49ce4105407" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "743b6cef-feb4-4ea7-afa1-507c68782d1c", "7b3ed1b7-38a6-4fcb-8021-d4ea5e2e0340" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
