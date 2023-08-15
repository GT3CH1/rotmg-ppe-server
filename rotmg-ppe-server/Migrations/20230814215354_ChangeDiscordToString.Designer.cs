// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using rotmg_ppe_server.data;

#nullable disable

namespace rotmg_ppe_server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230814215354_ChangeDiscordToString")]
    partial class ChangeDiscordToString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("ItemPlayer", b =>
                {
                    b.Property<int>("ItemsItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayersPlayerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemsItemId", "PlayersPlayerId");

                    b.HasIndex("PlayersPlayerId");

                    b.ToTable("ItemPlayer");
                });

            modelBuilder.Entity("rotmg_ppe_server.controllers.PendingRealmEyeUser", b =>
                {
                    b.Property<string>("DiscordId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountName")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("TEXT");

                    b.HasKey("DiscordId", "AccountName");

                    b.ToTable("PendingRealmEyeUsers");
                });

            modelBuilder.Entity("rotmg_ppe_server.models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Worth")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("rotmg_ppe_server.models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CharacterClass")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsDead")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsUpe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("rotmg_ppe_server.models.RealmEyeAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DiscordId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Verified")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("RealmEyeAccounts");
                });

            modelBuilder.Entity("ItemPlayer", b =>
                {
                    b.HasOne("rotmg_ppe_server.models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rotmg_ppe_server.models.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("rotmg_ppe_server.models.RealmEyeAccount", b =>
                {
                    b.HasOne("rotmg_ppe_server.models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
