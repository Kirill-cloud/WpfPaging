using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfPaging.Migrations
{
    public partial class OnDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditPlans_Banks_BankId",
                table: "CreditPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoringSystems_Banks_BankId",
                table: "ScoringSystems");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "ScoringSystems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "CreditPlans",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditPlans_Banks_BankId",
                table: "CreditPlans",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringSystems_Banks_BankId",
                table: "ScoringSystems",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditPlans_Banks_BankId",
                table: "CreditPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoringSystems_Banks_BankId",
                table: "ScoringSystems");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "ScoringSystems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "CreditPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CreditPlans_Banks_BankId",
                table: "CreditPlans",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringSystems_Banks_BankId",
                table: "ScoringSystems",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
