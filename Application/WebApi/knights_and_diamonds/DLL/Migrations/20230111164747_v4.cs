using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeckID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserHandID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Decks_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DeckID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hands_Decks_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Decks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hands_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckID",
                table: "Cards",
                column: "DeckID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserHandID",
                table: "Cards",
                column: "UserHandID");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserID",
                table: "Decks",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Hands_DeckID",
                table: "Hands",
                column: "DeckID");

            migrationBuilder.CreateIndex(
                name: "IX_Hands_UserID",
                table: "Hands",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Decks_DeckID",
                table: "Cards",
                column: "DeckID",
                principalTable: "Decks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_UserHandID",
                table: "Cards",
                column: "UserHandID",
                principalTable: "Hands",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Decks_DeckID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_UserHandID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Hands");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Cards_DeckID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserHandID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "DeckID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "UserHandID",
                table: "Cards");
        }
    }
}
