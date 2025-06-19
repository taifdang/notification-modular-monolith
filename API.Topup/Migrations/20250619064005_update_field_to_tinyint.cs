using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class update_field_to_tinyint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "itopup_status",
                table: "inbox_topup",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<byte>(
                name: "itopup_status",
                table: "inbox_notification",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "itopup_status",
                table: "inbox_topup",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "itopup_status",
                table: "inbox_notification",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
