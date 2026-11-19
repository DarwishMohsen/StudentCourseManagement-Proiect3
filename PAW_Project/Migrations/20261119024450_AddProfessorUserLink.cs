using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAW_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessorUserLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Professors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Professors",
                keyColumn: "PCNP",
                keyValue: "P101",
                columns: new[] { "Email", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Professors",
                keyColumn: "PCNP",
                keyValue: "P102",
                columns: new[] { "Email", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Professors",
                keyColumn: "PCNP",
                keyValue: "P103",
                columns: new[] { "Email", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Professors",
                keyColumn: "PCNP",
                keyValue: "P104",
                columns: new[] { "Email", "UserId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Professors_UserId",
                table: "Professors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professors_AspNetUsers_UserId",
                table: "Professors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professors_AspNetUsers_UserId",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_Professors_UserId",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Professors");
        }
    }
}
