using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Purchases_PurchaseId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Persons_PurchaseId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "PresentId",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PersonId",
                table: "Purchases",
                column: "PersonId");

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
                name: "FK_Purchases_Persons_PersonId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Presents_PresentId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PersonId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Purchases");

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
                name: "PurchaseId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PurchaseId",
                table: "Persons",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Purchases_PurchaseId",
                table: "Persons",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id");

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
