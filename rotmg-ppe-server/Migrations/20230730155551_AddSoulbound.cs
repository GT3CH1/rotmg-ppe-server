using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class AddSoulbound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // alter "Players" table to add "Soulbound" column
              migrationBuilder.AddColumn<bool>(
                name: "IsUpe",
                table: "Players",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            // alter "Items" table to add "Soulbound" column
            migrationBuilder.AddColumn<bool>(
                name: "Soulbound",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
