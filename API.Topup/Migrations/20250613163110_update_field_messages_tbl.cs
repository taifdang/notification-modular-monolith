using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class update_field_messages_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "action",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "detail",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "entity_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "messages",
                newName: "payload");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "payload",
                table: "messages",
                newName: "priority");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "detail",
                table: "messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "entity_id",
                table: "messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "messages",
                type: "int",
                nullable: true);
        }
    }
}
