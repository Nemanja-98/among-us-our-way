using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmongUs_OurWay.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false),
                    CrewmateGames = table.Column<int>(type: "int", nullable: false),
                    ImpostorGames = table.Column<int>(type: "int", nullable: false),
                    CrewmateWonGames = table.Column<int>(type: "int", nullable: false),
                    ImpostorWonGames = table.Column<int>(type: "int", nullable: false),
                    TasksCompleted = table.Column<int>(type: "int", nullable: false),
                    AllTaskCompleted = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Ref = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    User2Ref = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_User_User1Ref",
                        column: x => x.User1Ref,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_GameHistory_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameHistory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PendingRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSentRef = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserReceivedRef = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingRequest_User_UserReceivedRef",
                        column: x => x.UserReceivedRef,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PendingRequest_User_UserSentRef",
                        column: x => x.UserSentRef,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAction_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerAction_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_User1Ref",
                table: "Friend",
                column: "User1Ref");

            migrationBuilder.CreateIndex(
                name: "IX_GameHistory_GameId",
                table: "GameHistory",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameHistory_UserId",
                table: "GameHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingRequest_UserReceivedRef",
                table: "PendingRequest",
                column: "UserReceivedRef");

            migrationBuilder.CreateIndex(
                name: "IX_PendingRequest_UserSentRef",
                table: "PendingRequest",
                column: "UserSentRef");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAction_GameId",
                table: "PlayerAction",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAction_UserId",
                table: "PlayerAction",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "GameHistory");

            migrationBuilder.DropTable(
                name: "PendingRequest");

            migrationBuilder.DropTable(
                name: "PlayerAction");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
