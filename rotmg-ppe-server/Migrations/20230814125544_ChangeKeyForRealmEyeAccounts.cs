using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyForRealmEyeAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRealmEyeUsers",
                table: "PendingRealmEyeUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PendingRealmEyeUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordId",
                table: "PendingRealmEyeUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "PendingRealmEyeUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRealmEyeUsers",
                table: "PendingRealmEyeUsers",
                columns: new[] { "DiscordId", "AccountName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRealmEyeUsers",
                table: "PendingRealmEyeUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "PendingRealmEyeUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordId",
                table: "PendingRealmEyeUsers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PendingRealmEyeUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRealmEyeUsers",
                table: "PendingRealmEyeUsers",
                column: "Id");
        }
    }
}
