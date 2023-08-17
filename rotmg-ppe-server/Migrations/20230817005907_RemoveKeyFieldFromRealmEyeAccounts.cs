using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKeyFieldFromRealmEyeAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "RealmEyeAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RealmEyeAccounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
