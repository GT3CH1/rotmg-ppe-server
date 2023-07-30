using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSoulboundAddItemType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Soulbound",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Items");

            migrationBuilder.AddColumn<bool>(
                name: "Soulbound",
                table: "Items",
                type: "INTEGER",
                nullable: true);
        }
    }
}
