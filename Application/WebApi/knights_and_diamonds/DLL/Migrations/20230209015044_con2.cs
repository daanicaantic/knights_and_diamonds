using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class con2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreGameSessions_RockPaperScissorsGames_RPSGameID",
                table: "PreGameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_PreGameSessions_Users_UserID",
                table: "PreGameSessions");

            migrationBuilder.RenameColumn(
                name: "isStillLogeniIn",
                table: "Connections",
                newName: "TimeStamp");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "PreGameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RPSGameID",
                table: "PreGameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignalrID",
                table: "Connections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserID",
                table: "Connections",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_UserID",
                table: "Connections",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreGameSessions_RockPaperScissorsGames_RPSGameID",
                table: "PreGameSessions",
                column: "RPSGameID",
                principalTable: "RockPaperScissorsGames",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreGameSessions_Users_UserID",
                table: "PreGameSessions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_UserID",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_PreGameSessions_RockPaperScissorsGames_RPSGameID",
                table: "PreGameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_PreGameSessions_Users_UserID",
                table: "PreGameSessions");

            migrationBuilder.DropIndex(
                name: "IX_Connections_UserID",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "SignalrID",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Connections",
                newName: "isStillLogeniIn");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "PreGameSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RPSGameID",
                table: "PreGameSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PreGameSessions_RockPaperScissorsGames_RPSGameID",
                table: "PreGameSessions",
                column: "RPSGameID",
                principalTable: "RockPaperScissorsGames",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PreGameSessions_Users_UserID",
                table: "PreGameSessions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
