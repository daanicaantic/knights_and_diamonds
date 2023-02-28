using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class CardType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeID",
                table: "Cards",
                column: "CardTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CardTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardTypeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardTypeID",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
