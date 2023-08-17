using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealmEyeAccounts_Players_PlayerId",
                table: "RealmEyeAccounts");

            migrationBuilder.DropIndex(
                name: "IX_RealmEyeAccounts_PlayerId",
                table: "RealmEyeAccounts");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "RealmEyeAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "RealmEyeAccounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealmEyeAccounts_PlayerId",
                table: "RealmEyeAccounts",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealmEyeAccounts_Players_PlayerId",
                table: "RealmEyeAccounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId");
        }
    }
}
