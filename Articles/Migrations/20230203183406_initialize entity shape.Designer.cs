﻿// <auto-generated />
using System;
using Articles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Articles.Migrations
{
    [DbContext(typeof(ReportDbContext))]
    [Migration("20230203183406_initialize entity shape")]
    partial class initializeentityshape
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Articles.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CommentId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ReportId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Articles.Models.FollowedPeople", b =>
                {
                    b.Property<int>("ObserveId")
                        .HasColumnType("int");

                    b.Property<int>("TargetId")
                        .HasColumnType("int");

                    b.HasKey("ObserveId", "TargetId");

                    b.HasIndex("TargetId");

                    b.ToTable("FollowedPeoples");
                });

            modelBuilder.Entity("Articles.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"), 1L, 1);

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Hash")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Articles.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReportId"), 1L, 1);

                    b.Property<int?>("AuthorPersonId")
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ReportId");

                    b.HasIndex("AuthorPersonId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Articles.Models.ReportFavorite", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("ReportId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("ReportFavorites");
                });

            modelBuilder.Entity("Articles.Models.ReportTag", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReportId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ReportTags");
                });

            modelBuilder.Entity("Articles.Models.Tag", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Articles.Models.Comment", b =>
                {
                    b.HasOne("Articles.Models.Person", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Articles.Models.Report", "Report")
                        .WithMany("Comments")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Articles.Models.FollowedPeople", b =>
                {
                    b.HasOne("Articles.Models.Person", "Observer")
                        .WithMany("Followers")
                        .HasForeignKey("ObserveId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Articles.Models.Person", "Target")
                        .WithMany("Following")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Observer");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("Articles.Models.Report", b =>
                {
                    b.HasOne("Articles.Models.Person", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorPersonId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Articles.Models.ReportFavorite", b =>
                {
                    b.HasOne("Articles.Models.Person", "Person")
                        .WithMany("ReportFavorites")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Articles.Models.Report", "Report")
                        .WithMany("ReportFavorites")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Articles.Models.ReportTag", b =>
                {
                    b.HasOne("Articles.Models.Report", "Report")
                        .WithMany("ReportTags")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Articles.Models.Tag", "Tag")
                        .WithMany("ReportTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Articles.Models.Person", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("ReportFavorites");
                });

            modelBuilder.Entity("Articles.Models.Report", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ReportFavorites");

                    b.Navigation("ReportTags");
                });

            modelBuilder.Entity("Articles.Models.Tag", b =>
                {
                    b.Navigation("ReportTags");
                });
#pragma warning restore 612, 618
        }
    }
}
