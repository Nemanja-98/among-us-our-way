using Microsoft.EntityFrameworkCore.Migrations;

namespace AmongUs_OurWay.Migrations
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAction_User_UserId",
                table: "PlayerAction");

            migrationBuilder.DropIndex(
                name: "IX_PlayerAction_UserId",
                table: "PlayerAction");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlayerAction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlayerAction",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAction_UserId",
                table: "PlayerAction",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAction_User_UserId",
                table: "PlayerAction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
