﻿// <auto-generated />
using System;
using CKO.PaymentGateway.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CKO.PaymentGateway.DataAccess.Migrations
{
    [DbContext(typeof(PaymentGatewayContext))]
    [Migration("20221126113449_AddCardDetailsAndPayment")]
    partial class AddCardDetailsAndPayment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CKO.PaymentGateway.Domain.Cards.CardDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ExpiryMonth")
                        .HasColumnType("integer");

                    b.Property<int>("ExpiryYear")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CardDetails", (string)null);
                });

            modelBuilder.Entity("CKO.PaymentGateway.Domain.Payments.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CardDetailsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CardDetailsId")
                        .IsUnique();

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("CKO.PaymentGateway.Domain.Payments.Payment", b =>
                {
                    b.HasOne("CKO.PaymentGateway.Domain.Cards.CardDetails", "CardDetails")
                        .WithOne()
                        .HasForeignKey("CKO.PaymentGateway.Domain.Payments.Payment", "CardDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CKO.PaymentGateway.Domain.Payments.PaymentAmount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("PaymentId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Currency");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric")
                                .HasColumnName("Amount");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payment");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();

                    b.Navigation("CardDetails");
                });
#pragma warning restore 612, 618
        }
    }
}