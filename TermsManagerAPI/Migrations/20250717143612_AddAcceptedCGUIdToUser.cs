using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TermsManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAcceptedCGUIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptedCGUId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcceptedCGUId",
                table: "Users",
                column: "AcceptedCGUId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CGUs_AcceptedCGUId",
                table: "Users",
                column: "AcceptedCGUId",
                principalTable: "CGUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CGUs_AcceptedCGUId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AcceptedCGUId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AcceptedCGUId",
                table: "Users");
        }
    }
}
