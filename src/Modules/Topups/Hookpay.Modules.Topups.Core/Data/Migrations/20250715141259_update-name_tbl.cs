using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Topups.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatename_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TranferAmount",
                schema: "dbo",
                table: "Topup",
                newName: "TransferAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransferAmount",
                schema: "dbo",
                table: "Topup",
                newName: "TranferAmount");
        }
    }
}
