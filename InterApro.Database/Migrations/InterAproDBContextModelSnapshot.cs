﻿// <auto-generated />
using System;
using InterApro.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InterApro.Database.Migrations
{
    [DbContext(typeof(InterAproDBContext))]
    partial class InterAproDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InterApro.Database.Tables.Log", b =>
                {
                    b.Property<long>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogDescription")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(4096);

                    b.Property<long?>("RequestId")
                        .HasColumnType("bigint");

                    b.Property<long?>("RequestStatusId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LogId");

                    b.HasIndex("RequestId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("InterApro.Database.Tables.Request", b =>
                {
                    b.Property<long>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuyerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FinanceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ManagerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("RequestAmount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("RequestDescription")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(4096);

                    b.Property<long>("RequestStatusId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("RequestId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("InterApro.Database.Tables.RequestStatus", b =>
                {
                    b.Property<long>("RequestStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("RequestStatusId");

                    b.ToTable("RequestStatus");

                    b.HasData(
                        new
                        {
                            RequestStatusId = 1L,
                            Description = "New Request"
                        },
                        new
                        {
                            RequestStatusId = 2L,
                            Description = "Rejected by Manager"
                        },
                        new
                        {
                            RequestStatusId = 3L,
                            Description = "Approved by Manager"
                        },
                        new
                        {
                            RequestStatusId = 4L,
                            Description = "Rejected by Finance"
                        },
                        new
                        {
                            RequestStatusId = 5L,
                            Description = "Approved by Finance"
                        });
                });

            modelBuilder.Entity("InterApro.Database.Tables.Log", b =>
                {
                    b.HasOne("InterApro.Database.Tables.Request", null)
                        .WithMany("RequestLogs")
                        .HasForeignKey("RequestId");

                    b.HasOne("InterApro.Database.Tables.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId");
                });

            modelBuilder.Entity("InterApro.Database.Tables.Request", b =>
                {
                    b.HasOne("InterApro.Database.Tables.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
