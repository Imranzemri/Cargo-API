using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DMNSN",
                table: "SHIPMENT");

            migrationBuilder.DropColumn(
                name: "WGHT",
                table: "SHIPMENT");

            migrationBuilder.AddColumn<string>(
                name: "STS",
                table: "SHIPMENT",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DimensionArrayItem",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lngth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionArrayItem", x => new { x.ShipmentId, x.Id });
                    table.ForeignKey(
                        name: "FK_DimensionArrayItem_SHIPMENT_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "SHIPMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIXTURE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LNTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WDTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WGHT_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LNTH_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PRDT_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIXTURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK__FIXTURE__SHPT_NM__3A34245698",
                        column: x => x.SHPT_NMBR,
                        principalTable: "SHIPMENT",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateTable(
                name: "ORDERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOCN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMGS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSTM_RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true),
                    STS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDERS", x => x.ID);
                    table.UniqueConstraint("AK_ORDERS_SHPT_NMBR", x => x.SHPT_NMBR);
                });

            migrationBuilder.CreateTable(
                name: "RcptNumbers",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcptNumbers", x => new { x.ShipmentId, x.Id });
                    table.ForeignKey(
                        name: "FK_RcptNumbers_SHIPMENT_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "SHIPMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOCN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMGS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSTM_RPNT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true),
                    STS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFER", x => x.ID);
                    table.UniqueConstraint("AK_TRANSFER_SHPT_NMBR", x => x.SHPT_NMBR);
                });

            migrationBuilder.CreateTable(
                name: "WeightArrayItem",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Wght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightArrayItem", x => new { x.ShipmentId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeightArrayItem_SHIPMENT_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "SHIPMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimensionArrayItem_Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lngth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionArrayItem_Order", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_DimensionArrayItem_Order_ORDERS_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_FIXTURE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LNTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WDTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WGHT_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LNTH_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PRDT_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_FIXTURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK__ORDER_FIXTURE__SHPT_NM__3A34245698",
                        column: x => x.SHPT_NMBR,
                        principalTable: "ORDERS",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateTable(
                name: "ORDER_RECEIPT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_RECEIPT", x => x.ID);
                    table.ForeignKey(
                        name: "FK__ORDER_RECEIPT__SHPT_NM__3A342422",
                        column: x => x.SHPT_NMBR,
                        principalTable: "ORDERS",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateTable(
                name: "RcptNumbers_Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcptNumbers_Order", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_RcptNumbers_Order_ORDERS_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightArrayItem_Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Wght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightArrayItem_Order", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeightArrayItem_Order_ORDERS_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimensionArrayItem_Transfer",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lngth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionArrayItem_Transfer", x => new { x.TransferId, x.Id });
                    table.ForeignKey(
                        name: "FK_DimensionArrayItem_Transfer_TRANSFER_TransferId",
                        column: x => x.TransferId,
                        principalTable: "TRANSFER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    D_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CRIR_NME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LCNS_PLT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ID_IMG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RPNT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    TransferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Driver__SHPT_NMB__09A971A2",
                        column: x => x.SHPT_NMBR,
                        principalTable: "SHIPMENT",
                        principalColumn: "SHPT_NMBR");
                    table.ForeignKey(
                        name: "FK_Driver_ORDERS_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ORDERS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Driver_TRANSFER_TransferId",
                        column: x => x.TransferId,
                        principalTable: "TRANSFER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RcptNumbers_Transfer",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcptNumbers_Transfer", x => new { x.TransferId, x.Id });
                    table.ForeignKey(
                        name: "FK_RcptNumbers_Transfer_TRANSFER_TransferId",
                        column: x => x.TransferId,
                        principalTable: "TRANSFER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFER_FIXTURE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LNTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WDTH = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WGHT_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LNTH_UNIT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PRDT_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QNTY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFER_FIXTURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK__TRANSFER_FIXTURE__SHPT_NM__3A34245698",
                        column: x => x.SHPT_NMBR,
                        principalTable: "TRANSFER",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateTable(
                name: "TRANSFER_RECEIPT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RCPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SHPT_NMBR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFER_RECEIPT", x => x.ID);
                    table.ForeignKey(
                        name: "FK__TRANSFER_RECEIPT__SHPT_NM__3A81B327",
                        column: x => x.SHPT_NMBR,
                        principalTable: "TRANSFER",
                        principalColumn: "SHPT_NMBR");
                });

            migrationBuilder.CreateTable(
                name: "WeightArrayItem_Transfer",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Wght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RcptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShptNmbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ptype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qnty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightArrayItem_Transfer", x => new { x.TransferId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeightArrayItem_Transfer_TRANSFER_TransferId",
                        column: x => x.TransferId,
                        principalTable: "TRANSFER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Driver_OrderId",
                table: "Driver",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_SHPT_NMBR",
                table: "Driver",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_TransferId",
                table: "Driver",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_FIXTURE_SHPT_NMBR",
                table: "FIXTURE",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_FIXTURE_SHPT_NMBR",
                table: "ORDER_FIXTURE",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_RECEIPT_SHPT_NMBR",
                table: "ORDER_RECEIPT",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "UQ__ORDERS__5E9EFC15B8EA01E9",
                table: "ORDERS",
                column: "SHPT_NMBR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__TRANSFER__5E9EFC15B8EA01E9",
                table: "TRANSFER",
                column: "SHPT_NMBR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRANSFER_FIXTURE_SHPT_NMBR",
                table: "TRANSFER_FIXTURE",
                column: "SHPT_NMBR");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSFER_RECEIPT_SHPT_NMBR",
                table: "TRANSFER_RECEIPT",
                column: "SHPT_NMBR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DimensionArrayItem");

            migrationBuilder.DropTable(
                name: "DimensionArrayItem_Order");

            migrationBuilder.DropTable(
                name: "DimensionArrayItem_Transfer");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "FIXTURE");

            migrationBuilder.DropTable(
                name: "ORDER_FIXTURE");

            migrationBuilder.DropTable(
                name: "ORDER_RECEIPT");

            migrationBuilder.DropTable(
                name: "RcptNumbers");

            migrationBuilder.DropTable(
                name: "RcptNumbers_Order");

            migrationBuilder.DropTable(
                name: "RcptNumbers_Transfer");

            migrationBuilder.DropTable(
                name: "TRANSFER_FIXTURE");

            migrationBuilder.DropTable(
                name: "TRANSFER_RECEIPT");

            migrationBuilder.DropTable(
                name: "WeightArrayItem");

            migrationBuilder.DropTable(
                name: "WeightArrayItem_Order");

            migrationBuilder.DropTable(
                name: "WeightArrayItem_Transfer");

            migrationBuilder.DropTable(
                name: "ORDERS");

            migrationBuilder.DropTable(
                name: "TRANSFER");

            migrationBuilder.DropColumn(
                name: "STS",
                table: "SHIPMENT");

            migrationBuilder.AddColumn<decimal>(
                name: "DMNSN",
                table: "SHIPMENT",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WGHT",
                table: "SHIPMENT",
                type: "decimal(10,2)",
                nullable: true);
        }
    }
}
