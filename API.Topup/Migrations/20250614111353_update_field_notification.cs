using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class update_field_notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "messages",
                newName: "messsage_id");

            migrationBuilder.RenameColumn(
                name: "error",
                table: "inbox_notification",
                newName: "status");

            migrationBuilder.AddColumn<int>(
                name: "retry",
                table: "inbox_notification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "retry",
                table: "inbox_notification");

            migrationBuilder.RenameColumn(
                name: "messsage_id",
                table: "messages",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "inbox_notification",
                newName: "error");
        }
    }
}
