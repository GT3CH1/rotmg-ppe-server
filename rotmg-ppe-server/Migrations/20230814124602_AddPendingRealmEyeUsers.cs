using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingRealmEyeUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscordId",
                table: "RealmEyeAccounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PendingRealmEyeUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", nullable: true),
                    VerificationCode = table.Column<string>(type: "TEXT", nullable: true),
                    DiscordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingRealmEyeUsers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingRealmEyeUsers");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "RealmEyeAccounts");
        }
    }
}
