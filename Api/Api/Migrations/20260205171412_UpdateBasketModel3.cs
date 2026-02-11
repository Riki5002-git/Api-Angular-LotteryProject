using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBasketModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_PresentId",
                table: "BasketItem",
                column: "PresentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Presents_PresentId",
                table: "BasketItem");

            migrationBuilder.DropIndex(
                name: "IX_BasketItem_PresentId",
                table: "BasketItem");
        }
    }
}
