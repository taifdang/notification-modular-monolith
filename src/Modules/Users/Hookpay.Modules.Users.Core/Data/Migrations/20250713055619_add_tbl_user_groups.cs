using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Users.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_tbl_user_groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, defaultValue: ""),
                    user_password = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    user_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    user_email = table.Column<string>(type: "nvarchar(108)", maxLength: 108, nullable: false, defaultValue: ""),
                    user_phone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    is_block = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    set_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    set_user_id = table.Column<int>(type: "int", nullable: false),
                    disable_notification = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.set_id);
                    table.ForeignKey(
                        name: "FK_settings_users_set_user_id",
                        column: x => x.set_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_settings_set_user_id",
                table: "settings",
                column: "set_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_user_email",
                table: "users",
                column: "user_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
