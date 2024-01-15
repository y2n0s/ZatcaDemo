using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddInvoicesToZatca : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceToZatcas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DetailId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddressCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddressDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceCreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSalesInvoice = table.Column<bool>(type: "bit", nullable: true),
                    IsRefundInvoice = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxInvoice = table.Column<bool>(type: "bit", nullable: true),
                    IsSimplifiedInvoice = table.Column<bool>(type: "bit", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAddressCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAddressDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalForItems = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetWithoutVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoiceItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertFlag = table.Column<int>(type: "int", nullable: true),
                    UpdateFlag = table.Column<int>(type: "int", nullable: true),
                    DeleteFlag = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    ModifierId = table.Column<int>(type: "int", nullable: true),
                    PaymentMeans = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceToZatcas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceToZatcas");
        }
    }
}
