using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRealmEyeAccountKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealmEyeAccounts",
                table: "RealmEyeAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "DiscordId",
                table: "RealmEyeAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "RealmEyeAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RealmEyeAccounts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealmEyeAccounts",
                table: "RealmEyeAccounts",
                columns: new[] { "DiscordId", "AccountName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealmEyeAccounts",
                table: "RealmEyeAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RealmEyeAccounts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "RealmEyeAccounts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "DiscordId",
                table: "RealmEyeAccounts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealmEyeAccounts",
                table: "RealmEyeAccounts",
                column: "Id");
        }
    }
}
