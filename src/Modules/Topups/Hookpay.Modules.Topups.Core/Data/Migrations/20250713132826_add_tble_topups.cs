using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Topups.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_tble_topups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "topup",
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
                    table.PrimaryKey("PK_topup", x => x.topup_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topup_topup_trans_id",
                table: "topup",
                column: "topup_trans_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topup");
        }
    }
}
