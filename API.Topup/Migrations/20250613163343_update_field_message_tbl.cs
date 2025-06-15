using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class update_field_message_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "create_at",
                table: "messages",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "messages",
                newName: "create_at");
        }
    }
}
