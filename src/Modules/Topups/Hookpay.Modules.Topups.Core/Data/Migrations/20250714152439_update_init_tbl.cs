using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Topups.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_init_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Topup",
                schema: "dbo",
                columns: table => new
                {
                    topup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topup_trans_id = table.Column<int>(type: "int", nullable: false),
                    topup_source = table.Column<string>(type: "varchar(50)", nullable: true),
                    topup_creator = table.Column<string>(type: "varchar(20)", nullable: true),
                    topup_tranfer_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topup", x => x.topup_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topup_topup_trans_id",
                schema: "dbo",
                table: "Topup",
                column: "topup_trans_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topup",
                schema: "dbo");
        }
    }
}
