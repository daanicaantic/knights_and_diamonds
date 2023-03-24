using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class EffectChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_ElementTypes_ElementTypeID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_MonsterTypes_MonsterTypeID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "ElementTypes");

            migrationBuilder.DropTable(
                name: "MonsterTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ElementTypeID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_MonsterTypeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ElementTypeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MonsterTypeID",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementTypeID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonsterTypeID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ElementTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MonsterTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterTypes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ElementTypeID",
                table: "Cards",
                column: "ElementTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_MonsterTypeID",
                table: "Cards",
                column: "MonsterTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_ElementTypes_ElementTypeID",
                table: "Cards",
                column: "ElementTypeID",
                principalTable: "ElementTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_MonsterTypes_MonsterTypeID",
                table: "Cards",
                column: "MonsterTypeID",
                principalTable: "MonsterTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
