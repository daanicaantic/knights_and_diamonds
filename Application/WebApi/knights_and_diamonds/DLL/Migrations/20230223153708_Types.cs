using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class Types : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Effect",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ElementType",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "MonsterType",
                table: "Cards",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_ElementTypes_ElementTypeID",
                table: "Cards",
                column: "ElementTypeID",
                principalTable: "ElementTypes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_MonsterTypes_MonsterTypeID",
                table: "Cards",
                column: "MonsterTypeID",
                principalTable: "MonsterTypes",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

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

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Cards",
                newName: "MonsterType");

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Effect",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElementType",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
