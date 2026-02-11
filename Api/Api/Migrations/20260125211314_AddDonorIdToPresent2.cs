using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorIdToPresent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Categories_CategoryId",
                table: "Presents");

            migrationBuilder.DropIndex(
                name: "IX_Presents_CategoryId",
                table: "Presents");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
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
                name: "CategoryId",
                table: "Presents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Presents_CategoryId",
                table: "Presents",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Categories_CategoryId",
                table: "Presents",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
