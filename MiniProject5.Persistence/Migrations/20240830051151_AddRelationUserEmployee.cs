using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProject6.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationUserEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "employee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_userId",
                table: "employee",
                column: "userId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_AspNetUsers_userId",
                table: "employee",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_AspNetUsers_userId",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_userId",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "employee");
        }
    }
}
