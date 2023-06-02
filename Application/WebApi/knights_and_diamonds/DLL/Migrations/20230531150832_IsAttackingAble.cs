using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class IsAttackingAble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAttackingDone",
                table: "AttackInTurns",
                newName: "IsAttackingAbble");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAttackingAbble",
                table: "AttackInTurns",
                newName: "IsAttackingDone");
        }
    }
}
