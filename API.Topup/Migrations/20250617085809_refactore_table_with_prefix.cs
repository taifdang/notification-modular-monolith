using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class refactore_table_with_prefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbox_notification",
                columns: table => new
                {
                    inotify_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inotify_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inotify_event_type = table.Column<string>(type: "varchar(20)", nullable: false),
                    inotify_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    inotify_payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inotify_created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inotify_updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_notification", x => x.inotify_id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_topup",
                columns: table => new
                {
                    itopup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itopup_trans_id = table.Column<int>(type: "int", nullable: true),
                    itopup_event_type = table.Column<string>(type: "varchar(20)", nullable: false),
                    itopup_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    itopup_payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    itopup_created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    itopup_updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    itopup_error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_topup", x => x.itopup_id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    mess_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mess_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mess_event_type = table.Column<string>(type: "varchar(20)", nullable: false),
                    mess_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    mess_payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mess_created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.mess_id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_topup",
                columns: table => new
                {
                    otopup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    otopup_event_type = table.Column<string>(type: "varchar(20)", nullable: false),
                    otopup_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    otopup_payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otopup_created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    otopup_updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    otopup_status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_topup", x => x.otopup_id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    topup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topup_trans_id = table.Column<int>(type: "int", nullable: false),
                    topup_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    topup_creator = table.Column<string>(type: "varchar(20)", nullable: true),
                    topup_tranfer_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    topup_created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.topup_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "varchar(20)", nullable: false),
                    user_password = table.Column<string>(type: "varchar(25)", nullable: false),
                    user_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    user_email = table.Column<string>(type: "varchar(180)", nullable: true),
                    user_phone = table.Column<string>(type: "varchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_notification");

            migrationBuilder.DropTable(
                name: "inbox_topup");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "outbox_topup");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
