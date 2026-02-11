using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.CreateIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents",
                column: "WinnerId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
