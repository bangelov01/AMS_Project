using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Data.Migrations
{
    public partial class RemoveDeleteAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Auctions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Auctions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
