using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfPaging.Migrations
{
    public partial class CreditTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Target",
                table: "CreditPlans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Target",
                table: "CreditPlans");
        }
    }
}
