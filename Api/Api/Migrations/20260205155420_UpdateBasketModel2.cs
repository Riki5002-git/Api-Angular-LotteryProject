using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBasketModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Baskets_BasketId",
                table: "Presents");

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
                name: "IX_Presents_BasketId",
                table: "Presents");

            migrationBuilder.DropIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Baskets");

            migrationBuilder.AlterColumn<int>(
                name: "PresentId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WinnerId",
                table: "Presents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BasketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketId",
                table: "BasketItem",
                column: "BasketId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.AlterColumn<int>(
                name: "PresentId",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WinnerId",
                table: "Presents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Presents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Baskets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Presents_BasketId",
                table: "Presents",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Baskets_BasketId",
                table: "Presents",
                column: "BasketId",
                principalTable: "Baskets",
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
    }
}
