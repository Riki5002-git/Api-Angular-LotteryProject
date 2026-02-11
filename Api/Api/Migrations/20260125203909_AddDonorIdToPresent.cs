using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorIdToPresent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Donors_DonorId",
                table: "Presents");

            migrationBuilder.DropIndex(
                name: "IX_Presents_DonorId",
                table: "Presents");

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "Presents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "Presents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Presents_DonorId",
                table: "Presents",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Donors_DonorId",
                table: "Presents",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id");
        }
    }
}
