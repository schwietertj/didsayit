using Microsoft.EntityFrameworkCore.Migrations;

namespace DidSayItModels.Migrations
{
    public partial class addColumnContentPublish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Publish",
                table: "Contents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publish",
                table: "Contents");
        }
    }
}
