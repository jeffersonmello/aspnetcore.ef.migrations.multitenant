﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MultiTenant.Data.Context;

#nullable disable

namespace MultiTenant.Data.Migrations.Log
{
    [DbContext(typeof(LogContext))]
    [Migration("20220105133207_LogInitialMigration")]
    partial class LogInitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MultiTenant.Model.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Acontecimento")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("acontecimento");

                    b.Property<string>("Dados")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("dados");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("datahora");

                    b.Property<string>("RequestId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("request_id");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("schema");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuarioid");

                    b.HasKey("Id");

                    b.ToTable("logs", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}