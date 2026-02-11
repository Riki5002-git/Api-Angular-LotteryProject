using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Presents",
                newName: "WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Presents_PersonId",
                table: "Presents",
                newName: "IX_Presents_WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents",
                column: "WinnerId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Presents",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents",
                newName: "IX_Presents_PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
