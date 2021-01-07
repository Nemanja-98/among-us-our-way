﻿// <auto-generated />
using System;
using AmongUs_OurWay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AmongUs_OurWay.Migrations
{
    [DbContext(typeof(AmongUsContext))]
    [Migration("20210107215219_V1.0")]
    partial class V10
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("AmongUs_OurWay.Models.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .UseIdentityColumn();

                    b.Property<string>("User1Ref")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("User1Ref");

                    b.Property<string>("User2Ref")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("User2Ref");

                    b.HasKey("Id");

                    b.HasIndex("User1Ref");

                    b.ToTable("Friend");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateStarted")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateStarted");

                    b.HasKey("Id");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.GameHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .UseIdentityColumn();

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("GameId");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("GameHistory");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.PendingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .UseIdentityColumn();

                    b.Property<string>("UserReceivedRef")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserReceivedRef");

                    b.Property<string>("UserSentRef")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserSentRef");

                    b.HasKey("Id");

                    b.HasIndex("UserReceivedRef");

                    b.HasIndex("UserSentRef");

                    b.ToTable("PendingRequest");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.PlayerAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .UseIdentityColumn();

                    b.Property<int>("Action")
                        .HasColumnType("int")
                        .HasColumnName("Action");

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("GameId");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time")
                        .HasColumnName("Time");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("PlayerAction");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Username");

                    b.Property<int>("AllTasksCompleted")
                        .HasColumnType("int")
                        .HasColumnName("AllTaskCompleted");

                    b.Property<int>("CrewmateGames")
                        .HasColumnType("int")
                        .HasColumnName("CrewmateGames");

                    b.Property<int>("CrewmateWonGames")
                        .HasColumnType("int")
                        .HasColumnName("CrewmateWonGames");

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int")
                        .HasColumnName("GamesPlayed");

                    b.Property<int>("ImpostorGames")
                        .HasColumnType("int")
                        .HasColumnName("ImpostorGames");

                    b.Property<int>("ImpostorWonGames")
                        .HasColumnType("int")
                        .HasColumnName("ImpostorWonGames");

                    b.Property<int>("Kills")
                        .HasColumnType("int")
                        .HasColumnName("Kills");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<int>("TasksCompleted")
                        .HasColumnType("int")
                        .HasColumnName("TasksCompleted");

                    b.HasKey("Username");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.Friend", b =>
                {
                    b.HasOne("AmongUs_OurWay.Models.User", "User1")
                        .WithMany("Friends")
                        .HasForeignKey("User1Ref")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("User1");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.GameHistory", b =>
                {
                    b.HasOne("AmongUs_OurWay.Models.Game", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("AmongUs_OurWay.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.PendingRequest", b =>
                {
                    b.HasOne("AmongUs_OurWay.Models.User", "UserReceived")
                        .WithMany("PendingRequests")
                        .HasForeignKey("UserReceivedRef")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("AmongUs_OurWay.Models.User", "UserSent")
                        .WithMany("SentRequests")
                        .HasForeignKey("UserSentRef")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("UserReceived");

                    b.Navigation("UserSent");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.PlayerAction", b =>
                {
                    b.HasOne("AmongUs_OurWay.Models.Game", "Game")
                        .WithMany("Actions")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("AmongUs_OurWay.Models.User", "User")
                        .WithMany("Actions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.Game", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("AmongUs_OurWay.Models.User", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("Friends");

                    b.Navigation("Games");

                    b.Navigation("PendingRequests");

                    b.Navigation("SentRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
