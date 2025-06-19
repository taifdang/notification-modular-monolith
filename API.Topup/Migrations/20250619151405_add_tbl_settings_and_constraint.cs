using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class add_tbl_settings_and_constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "mess_user_id",
                table: "messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    set_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    set_user_id = table.Column<int>(type: "int", nullable: false),
                    disable_notification = table.Column<bool>(type: "bit", nullable: false)
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
                name: "IX_messages_mess_user_id",
                table: "messages",
                column: "mess_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_settings_set_user_id",
                table: "settings",
                column: "set_user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_mess_user_id",
                table: "messages",
                column: "mess_user_id",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_mess_user_id",
                table: "messages");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropIndex(
                name: "IX_messages_mess_user_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "mess_user_id",
                table: "messages");
        }
    }
}
