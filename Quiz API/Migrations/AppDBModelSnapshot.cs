﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quiz_API.Entity;

#nullable disable

namespace QuizAPI.Migrations
{
    [DbContext(typeof(AppDB))]
    partial class AppDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryQuestion", b =>
                {
                    b.Property<int>("CategorysId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionsId")
                        .HasColumnType("int");

                    b.HasKey("CategorysId", "QuestionsId");

                    b.HasIndex("QuestionsId");

                    b.ToTable("CategoryQuestion");
                });

            modelBuilder.Entity("QuestionUser", b =>
                {
                    b.Property<int>("QuestionsListId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("QuestionsListId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("QuestionUser");
                });

            modelBuilder.Entity("Quiz_API.Entity.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("answers");
                });

            modelBuilder.Entity("Quiz_API.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Quiz_API.Entity.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("companys");
                });

            modelBuilder.Entity("Quiz_API.Entity.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Question");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Quiz_API.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Quiz_API.Entity.EasyQuestion", b =>
                {
                    b.HasBaseType("Quiz_API.Entity.Question");

                    b.HasDiscriminator().HasValue("EasyQuestion");
                });

            modelBuilder.Entity("Quiz_API.Entity.HardQuestion", b =>
                {
                    b.HasBaseType("Quiz_API.Entity.Question");

                    b.HasDiscriminator().HasValue("HardQuestion");
                });

            modelBuilder.Entity("Quiz_API.Entity.MidQuestion", b =>
                {
                    b.HasBaseType("Quiz_API.Entity.Question");

                    b.HasDiscriminator().HasValue("MidQuestion");
                });

            modelBuilder.Entity("Quiz_API.Entity.CompanyUser", b =>
                {
                    b.HasBaseType("Quiz_API.Entity.User");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.HasIndex("CompanyId");

                    b.HasDiscriminator().HasValue("CompanyUser");
                });

            modelBuilder.Entity("Quiz_API.Entity.PrivateUser", b =>
                {
                    b.HasBaseType("Quiz_API.Entity.User");

                    b.HasDiscriminator().HasValue("PrivateUser");
                });

            modelBuilder.Entity("CategoryQuestion", b =>
                {
                    b.HasOne("Quiz_API.Entity.Category", null)
                        .WithMany()
                        .HasForeignKey("CategorysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz_API.Entity.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuestionUser", b =>
                {
                    b.HasOne("Quiz_API.Entity.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz_API.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Quiz_API.Entity.Answer", b =>
                {
                    b.HasOne("Quiz_API.Entity.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Quiz_API.Entity.CompanyUser", b =>
                {
                    b.HasOne("Quiz_API.Entity.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Quiz_API.Entity.Company", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Quiz_API.Entity.Question", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
