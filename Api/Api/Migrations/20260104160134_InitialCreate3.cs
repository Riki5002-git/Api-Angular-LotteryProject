using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Presents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Presents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchasesAmount",
                table: "Presents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Categories_CategoryId",
                table: "Presents");

            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Presents_CategoryId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "PurchasesAmount",
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
    }
}
