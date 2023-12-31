﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ThirdProject_BackEnd.Data;

#nullable disable

namespace ThirdProject_BackEnd.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231027014101_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ThirdProject_BackEnd.Models.RefreshToken", b =>
                {
                    b.Property<string>("user_name")
                        .HasColumnType("text");

                    b.Property<DateTime>("create_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("expires_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("user_name");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ThirdProject_BackEnd.Models.User", b =>
                {
                    b.Property<string>("user_name")
                        .HasColumnType("text");

                    b.Property<DateTime>("birth_day")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("create_time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("last_login")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("password_hash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("password_salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("user_name");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ThirdProject_BackEnd.Models.RefreshToken", b =>
                {
                    b.HasOne("ThirdProject_BackEnd.Models.User", "user")
                        .WithOne("RefreshToken")
                        .HasForeignKey("ThirdProject_BackEnd.Models.RefreshToken", "user_name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("ThirdProject_BackEnd.Models.User", b =>
                {
                    b.Navigation("RefreshToken")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
