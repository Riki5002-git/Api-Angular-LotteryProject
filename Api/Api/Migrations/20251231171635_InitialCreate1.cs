using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Persons_WinnerId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_WinnerId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Presents",
                newName: "WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Presents_PersonId",
                table: "Presents",
                newName: "IX_Presents_WinnerId");

            migrationBuilder.AlterColumn<int>(
                name: "PresentId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonorId",
                table: "Presents",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Persons",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presents_DonorId",
                table: "Presents",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Email",
                table: "Persons",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Password",
                table: "Persons",
                column: "Password",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Phone",
                table: "Persons",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UserName",
                table: "Persons",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Donors_DonorId",
                table: "Presents",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents",
                column: "WinnerId",
                principalTable: "Persons",
                principalColumn: "Id");

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
                name: "FK_Presents_Donors_DonorId",
                table: "Presents");

            migrationBuilder.DropForeignKey(
                name: "FK_Presents_Persons_WinnerId",
                table: "Presents");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_Presents_DonorId",
                table: "Presents");

            migrationBuilder.DropIndex(
                name: "IX_Persons_Email",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_Password",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_Phone",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_UserName",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "Presents");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Presents",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Presents_WinnerId",
                table: "Presents",
                newName: "IX_Presents_PersonId");

            migrationBuilder.AlterColumn<int>(
                name: "PresentId",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_WinnerId",
                table: "Purchases",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presents_Persons_PersonId",
                table: "Presents",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Persons_WinnerId",
                table: "Purchases",
                column: "WinnerId",
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
