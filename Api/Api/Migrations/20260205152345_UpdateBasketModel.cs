using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBasketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Persons_OwnerId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Presents_PresentId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_OwnerId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_PresentId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "PresentId",
                table: "Baskets");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Presents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Baskets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Presents_BasketId",
                table: "Presents",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Baskets_BasketId",
                table: "Presents",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Baskets_BasketId",
                table: "Presents");

            migrationBuilder.DropIndex(
                name: "IX_Presents_BasketId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Baskets");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Baskets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentId",
                table: "Baskets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_OwnerId",
                table: "Baskets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_PresentId",
                table: "Baskets",
                column: "PresentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Persons_OwnerId",
                table: "Baskets",
                column: "OwnerId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Presents_PresentId",
                table: "Baskets",
                column: "PresentId",
                principalTable: "Presents",
                principalColumn: "Id");
        }
    }
}
