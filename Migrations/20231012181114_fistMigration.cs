using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoApi.Migrations
{
    public partial class fistMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SHIPMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DMNSN = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    WGHT = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    LOCN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMGS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSTM_RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPMENT", x => x.ID);
                    table.UniqueConstraint("AK_SHIPMENT_SHPT_NMBR", x => x.SHPT_NMBR);
                });

            migrationBuilder.CreateTable(
                name: "RECEIPT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECEIPT", x => x.ID);
                    table.ForeignKey(
                        name: "FK__RECEIPT__SHPT_NM__3A81B327",
                        column: x => x.SHPT_NMBR,
                        principalTable: "SHIPMENT",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RECEIPT_SHPT_NMBR",
                table: "RECEIPT",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "UQ__SHIPMENT__5E9EFC15B8EA01E9",
                table: "SHIPMENT",
                column: "SHPT_NMBR",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RECEIPT");

            migrationBuilder.DropTable(
                name: "SHIPMENT");
        }
    }
}
