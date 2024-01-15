﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Domain.Entities.Fodo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Data.Fodo;

public partial class FodoDbContext : DbContext
{ 
    public FodoDbContext(DbContextOptions<FodoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InvoicesToZATCA> InvoicesToZATCAs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<InvoicesToZATCA>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("InvoicesToZATCA_pk");

            entity.ToTable("InvoicesToZATCA");

            entity.Property(e => e.DetailId).ValueGeneratedNever();
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.DeleteFlag).HasDefaultValueSql("((0))");
            entity.Property(e => e.InsertFlag).HasDefaultValueSql("((1))");
            entity.Property(e => e.InvoiceCreationDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.NetWithoutVAT).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxPercentage).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalDiscount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalForItems).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UpdateFlag).HasDefaultValueSql("((0))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}