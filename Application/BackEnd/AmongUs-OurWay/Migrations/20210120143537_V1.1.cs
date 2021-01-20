using Microsoft.EntityFrameworkCore.Migrations;

namespace AmongUs_OurWay.Migrations
{
    public partial class V11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingRequest_User_UserReceivedRef",
                table: "PendingRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingRequest_User_UserSentRef",
                table: "PendingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRequest",
                table: "PendingRequest");

            migrationBuilder.RenameTable(
                name: "PendingRequest",
                newName: "PendingRequests");

            migrationBuilder.RenameIndex(
                name: "IX_PendingRequest_UserSentRef",
                table: "PendingRequests",
                newName: "IX_PendingRequests_UserSentRef");

            migrationBuilder.RenameIndex(
                name: "IX_PendingRequest_UserReceivedRef",
                table: "PendingRequests",
                newName: "IX_PendingRequests_UserReceivedRef");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRequests",
                table: "PendingRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingRequests_User_UserReceivedRef",
                table: "PendingRequests",
                column: "UserReceivedRef",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingRequests_User_UserSentRef",
                table: "PendingRequests",
                column: "UserSentRef",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingRequests_User_UserReceivedRef",
                table: "PendingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingRequests_User_UserSentRef",
                table: "PendingRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRequests",
                table: "PendingRequests");

            migrationBuilder.RenameTable(
                name: "PendingRequests",
                newName: "PendingRequest");

            migrationBuilder.RenameIndex(
                name: "IX_PendingRequests_UserSentRef",
                table: "PendingRequest",
                newName: "IX_PendingRequest_UserSentRef");

            migrationBuilder.RenameIndex(
                name: "IX_PendingRequests_UserReceivedRef",
                table: "PendingRequest",
                newName: "IX_PendingRequest_UserReceivedRef");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRequest",
                table: "PendingRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingRequest_User_UserReceivedRef",
                table: "PendingRequest",
                column: "UserReceivedRef",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingRequest_User_UserSentRef",
                table: "PendingRequest",
                column: "UserSentRef",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
