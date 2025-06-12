using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Topup.Migrations
{
    /// <inheritdoc />
    public partial class initial_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbox_topup",
                columns: table => new
                {
                    topup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topup_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    process_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_topup", x => x.topup_id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_topup",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    processed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_topup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<int>(type: "int", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tranfer_amount = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount_coint = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_topup");

            migrationBuilder.DropTable(
                name: "outbox_topup");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
