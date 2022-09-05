using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfPaging.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimalScore = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    BankId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditPlans_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScoringSystems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(nullable: true),
                    MinCondition = table.Column<int>(nullable: true),
                    MaxCondition = table.Column<int>(nullable: true),
                    ExactValue = table.Column<int>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringSystems_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditPlans_BankId",
                table: "CreditPlans",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringSystems_BankId",
                table: "ScoringSystems",
                column: "BankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditPlans");

            migrationBuilder.DropTable(
                name: "ScoringSystems");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
