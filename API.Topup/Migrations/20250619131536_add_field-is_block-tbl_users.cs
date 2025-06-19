using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class add_fieldis_blocktbl_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_block",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_block",
                table: "users");
        }
    }
}
