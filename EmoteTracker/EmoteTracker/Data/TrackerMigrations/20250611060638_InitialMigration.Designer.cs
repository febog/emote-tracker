﻿// <auto-generated />
using EmoteTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmoteTracker.Data.TrackerMigrations
{
    [DbContext(typeof(EmoteTrackerContext))]
    [Migration("20250611060638_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmoteTracker.Models.TwitchChannel", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("TwitchChannels");
                });

            modelBuilder.Entity("EmoteTracker.Models.TwitchChannelEmote", b =>
                {
                    b.Property<string>("EmoteId")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnOrder(1);

                    b.Property<string>("TwitchChannelId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnOrder(2);

                    b.Property<string>("Alias")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CanonicalName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("EmoteType")
                        .HasColumnType("int");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsListed")
                        .HasColumnType("bit");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("EmoteId", "TwitchChannelId");

                    b.HasIndex("TwitchChannelId");

                    b.ToTable("TwitchChannelEmotes");
                });

            modelBuilder.Entity("EmoteTracker.Models.TwitchChannelEmote", b =>
                {
                    b.HasOne("EmoteTracker.Models.TwitchChannel", "TwitchChannel")
                        .WithMany("TwitchChannelEmotes")
                        .HasForeignKey("TwitchChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TwitchChannel");
                });

            modelBuilder.Entity("EmoteTracker.Models.TwitchChannel", b =>
                {
                    b.Navigation("TwitchChannelEmotes");
                });
#pragma warning restore 612, 618
        }
    }
}
