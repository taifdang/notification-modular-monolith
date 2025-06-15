using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class addinbox_notification_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "message_type",
                table: "outbox_topup",
                newName: "event_type");

            migrationBuilder.RenameColumn(
                name: "topup_type",
                table: "inbox_topup",
                newName: "event_type");

            migrationBuilder.RenameColumn(
                name: "topup_id",
                table: "inbox_topup",
                newName: "id");

            migrationBuilder.CreateTable(
                name: "inbox_notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    processed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_notification", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_notification");

            migrationBuilder.RenameColumn(
                name: "event_type",
                table: "outbox_topup",
                newName: "message_type");

            migrationBuilder.RenameColumn(
                name: "event_type",
                table: "inbox_topup",
                newName: "topup_type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "inbox_topup",
                newName: "topup_id");
        }
    }
}
