using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Notifications.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_tbl_message_group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inboxMessage",
                columns: table => new
                {
                    correlationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    eventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    processed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inboxMessage", x => x.correlationId);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    mess_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mess_correlationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mess_userId = table.Column<int>(type: "int", nullable: false),
                    mess_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mess_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mess_createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mess_processed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.mess_id);
                });

            migrationBuilder.CreateTable(
                name: "outboxMessage",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correlationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outboxMessage", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_mess_correlationId",
                table: "message",
                column: "mess_correlationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inboxMessage");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "outboxMessage");
        }
    }
}
