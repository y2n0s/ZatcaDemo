﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Data;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240109125923_AddInvoicesToZatca")]
    partial class AddInvoicesToZatca
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.CertificateSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Certificate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CertificateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Csr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpiredDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Secret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CertificateSettings");
                });

            modelBuilder.Entity("Domain.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InvoiceHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvoiceType")
                        .HasColumnType("int");

                    b.Property<int>("InvoiceTypeCode")
                        .HasColumnType("int");

                    b.Property<string>("QrCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReportedToZatca")
                        .HasColumnType("bit");

                    b.Property<string>("ReportingResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignedXml")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Domain.Entities.InvoiceToZatca", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyAddressCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyAddressDistrict")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyTaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerAddressCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerAddressDistrict")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DeleteFlag")
                        .HasColumnType("int");

                    b.Property<long>("DetailId")
                        .HasColumnType("bigint");

                    b.Property<int?>("InsertFlag")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InvoiceCreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("InvoiceDeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("InvoiceId")
                        .HasColumnType("bigint");

                    b.Property<string>("InvoiceItemsJson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsRefundInvoice")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSalesInvoice")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSent")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSimplifiedInvoice")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsTaxInvoice")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ModifierId")
                        .HasColumnType("int");

                    b.Property<decimal?>("NetWithoutVAT")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PaymentMeans")
                        .HasColumnType("int");

                    b.Property<string>("RefundReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TaxAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TaxPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalForItems")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UpdateFlag")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("InvoiceToZatcas");
                });

            modelBuilder.Entity("Domain.Entities.InvoiceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InvoiceTypes");
                });

            modelBuilder.Entity("Domain.Entities.Seller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalStreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SellerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SellerTRN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("Domain.Entities.SellerIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SellerIdentities");
                });
#pragma warning restore 612, 618
        }
    }
}