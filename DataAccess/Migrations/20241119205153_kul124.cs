using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class kul124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar");

            migrationBuilder.AddForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar");

            migrationBuilder.AddForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id");
        }
    }
}
