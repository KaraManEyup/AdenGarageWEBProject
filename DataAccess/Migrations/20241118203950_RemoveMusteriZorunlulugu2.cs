using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMusteriZorunlulugu2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar");

            migrationBuilder.AlterColumn<int>(
                name: "MusteriId",
                table: "Arabalar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar");

            migrationBuilder.AlterColumn<int>(
                name: "MusteriId",
                table: "Arabalar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Arabalar_Musteriler_MusteriId",
                table: "Arabalar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
