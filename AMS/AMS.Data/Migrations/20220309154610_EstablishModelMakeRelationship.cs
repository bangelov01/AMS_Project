using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Data.Migrations
{
    public partial class EstablishModelMakeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Makes_MakeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_MakeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "Models",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId",
                table: "Models",
                column: "MakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Makes_MakeId",
                table: "Models",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Makes_MakeId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_MakeId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "Models");

            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_MakeId",
                table: "Vehicles",
                column: "MakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Makes_MakeId",
                table: "Vehicles",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
