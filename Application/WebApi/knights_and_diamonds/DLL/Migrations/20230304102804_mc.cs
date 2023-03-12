using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfStars",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DefencePoints",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttackPoints",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfStars",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DefencePoints",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AttackPoints",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
