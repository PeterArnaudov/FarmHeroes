﻿// <auto-generated />
using System;
using FarmHeroes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FarmHeroes.Data.Migrations
{
    [DbContext(typeof(FarmHeroesDbContext))]
    partial class FarmHeroesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FarmHeroes.Data.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.FightModels.Fight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttackerAttack")
                        .HasColumnType("int");

                    b.Property<string>("AttackerAvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AttackerDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("AttackerDefense")
                        .HasColumnType("int");

                    b.Property<int>("AttackerHealthLeft")
                        .HasColumnType("int");

                    b.Property<int?>("AttackerHitFive")
                        .HasColumnType("int");

                    b.Property<int?>("AttackerHitFour")
                        .HasColumnType("int");

                    b.Property<int?>("AttackerHitOne")
                        .HasColumnType("int");

                    b.Property<int?>("AttackerHitThree")
                        .HasColumnType("int");

                    b.Property<int?>("AttackerHitTwo")
                        .HasColumnType("int");

                    b.Property<int>("AttackerId")
                        .HasColumnType("int");

                    b.Property<int>("AttackerLevel")
                        .HasColumnType("int");

                    b.Property<int>("AttackerMass")
                        .HasColumnType("int");

                    b.Property<int>("AttackerMastery")
                        .HasColumnType("int");

                    b.Property<string>("AttackerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DefenderAttack")
                        .HasColumnType("int");

                    b.Property<string>("DefenderAvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DefenderDamageDealt")
                        .HasColumnType("int");

                    b.Property<int>("DefenderDefense")
                        .HasColumnType("int");

                    b.Property<int>("DefenderHealthLeft")
                        .HasColumnType("int");

                    b.Property<int?>("DefenderHitFive")
                        .HasColumnType("int");

                    b.Property<int?>("DefenderHitFour")
                        .HasColumnType("int");

                    b.Property<int?>("DefenderHitOne")
                        .HasColumnType("int");

                    b.Property<int?>("DefenderHitThree")
                        .HasColumnType("int");

                    b.Property<int?>("DefenderHitTwo")
                        .HasColumnType("int");

                    b.Property<int>("DefenderId")
                        .HasColumnType("int");

                    b.Property<int>("DefenderLevel")
                        .HasColumnType("int");

                    b.Property<int>("DefenderMass")
                        .HasColumnType("int");

                    b.Property<int>("DefenderMastery")
                        .HasColumnType("int");

                    b.Property<string>("DefenderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExperienceGained")
                        .HasColumnType("int");

                    b.Property<int>("GoldStolen")
                        .HasColumnType("int");

                    b.Property<string>("WinnerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Fights");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Characteristics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Attack")
                        .HasColumnType("int");

                    b.Property<int>("Defense")
                        .HasColumnType("int");

                    b.Property<int>("Mass")
                        .HasColumnType("int");

                    b.Property<int>("Mastery")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Characteristics");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Chronometer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CannotAttackHeroUntil")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CannotAttackMonsterUntil")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CannotBeAttackedUntil")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WorkUntil")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Chronometers");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.EquippedSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArmorId")
                        .HasColumnType("int");

                    b.Property<int?>("HelmetId")
                        .HasColumnType("int");

                    b.Property<int?>("ShieldId")
                        .HasColumnType("int");

                    b.Property<int?>("WeaponId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArmorId")
                        .IsUnique()
                        .HasFilter("[ArmorId] IS NOT NULL");

                    b.HasIndex("HelmetId")
                        .IsUnique()
                        .HasFilter("[HelmetId] IS NOT NULL");

                    b.HasIndex("ShieldId")
                        .IsUnique()
                        .HasFilter("[ShieldId] IS NOT NULL");

                    b.HasIndex("WeaponId")
                        .IsUnique()
                        .HasFilter("[WeaponId] IS NOT NULL");

                    b.ToTable("EquippedSets");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Health", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Current")
                        .HasColumnType("int");

                    b.Property<int>("Maximum")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Healths");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Hero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CharacteristicsId")
                        .HasColumnType("int");

                    b.Property<int>("ChronometerId")
                        .HasColumnType("int");

                    b.Property<int>("EquippedSetId")
                        .HasColumnType("int");

                    b.Property<int>("Fraction")
                        .HasColumnType("int");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<int>("HealthId")
                        .HasColumnType("int");

                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResourcePouchId")
                        .HasColumnType("int");

                    b.Property<int>("StatisticsId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacteristicsId")
                        .IsUnique();

                    b.HasIndex("ChronometerId")
                        .IsUnique();

                    b.HasIndex("EquippedSetId")
                        .IsUnique();

                    b.HasIndex("HealthId")
                        .IsUnique();

                    b.HasIndex("InventoryId")
                        .IsUnique();

                    b.HasIndex("LevelId")
                        .IsUnique();

                    b.HasIndex("ResourcePouchId")
                        .IsUnique();

                    b.HasIndex("StatisticsId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Bonus")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("RequiredLevel")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.ToTable("HeroEquipments");

                    b.HasDiscriminator<string>("Discriminator").HasValue("HeroEquipment");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ItemsCap")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CurrentExperience")
                        .HasColumnType("int");

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<int>("NeededExperience")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.ResourcePouch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Crystals")
                        .HasColumnType("int");

                    b.Property<int>("Gold")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ResourcePouches");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Statistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EarnedFromMonsters")
                        .HasColumnType("int");

                    b.Property<int>("EarnedInMines")
                        .HasColumnType("int");

                    b.Property<int>("EarnedOnFarm")
                        .HasColumnType("int");

                    b.Property<int>("EarnedOnPatrol")
                        .HasColumnType("int");

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<int>("MaximalGoldSteal")
                        .HasColumnType("int");

                    b.Property<int>("MonstersDefeated")
                        .HasColumnType("int");

                    b.Property<int>("TotalCrystalsStolen")
                        .HasColumnType("int");

                    b.Property<int>("TotalFights")
                        .HasColumnType("int");

                    b.Property<int>("TotalGoldStolen")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.MappingModels.HeroFight", b =>
                {
                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<int>("FightId")
                        .HasColumnType("int");

                    b.HasKey("HeroId", "FightId");

                    b.HasIndex("FightId");

                    b.ToTable("HeroFights");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.MonsterModels.Monster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatPercentage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.NotificationModels.HeroModels.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Experience")
                        .HasColumnType("int");

                    b.Property<int?>("Gold")
                        .HasColumnType("int");

                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsNew")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HeroId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ShopModels.ShopEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Bonus")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("RequiredLevel")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("ShopEquipments");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ShopEquipment");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroArmor", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.HeroModels.HeroEquipment");

                    b.HasDiscriminator().HasValue("HeroArmor");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroHelmet", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.HeroModels.HeroEquipment");

                    b.HasDiscriminator().HasValue("HeroHelmet");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroShield", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.HeroModels.HeroEquipment");

                    b.HasDiscriminator().HasValue("HeroShield");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroWeapon", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.HeroModels.HeroEquipment");

                    b.HasDiscriminator().HasValue("HeroWeapon");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ShopModels.ShopArmor", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.ShopModels.ShopEquipment");

                    b.HasDiscriminator().HasValue("ShopArmor");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ShopModels.ShopHelmet", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.ShopModels.ShopEquipment");

                    b.HasDiscriminator().HasValue("ShopHelmet");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ShopModels.ShopShield", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.ShopModels.ShopEquipment");

                    b.HasDiscriminator().HasValue("ShopShield");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.ShopModels.ShopWeapon", b =>
                {
                    b.HasBaseType("FarmHeroes.Data.Models.ShopModels.ShopEquipment");

                    b.HasDiscriminator().HasValue("ShopWeapon");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.EquippedSet", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.HeroModels.HeroArmor", "Armor")
                        .WithOne("EquippedSet")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.EquippedSet", "ArmorId");

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.HeroHelmet", "Helmet")
                        .WithOne("EquippedSet")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.EquippedSet", "HelmetId");

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.HeroShield", "Shield")
                        .WithOne("EquippedSet")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.EquippedSet", "ShieldId");

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.HeroWeapon", "Weapon")
                        .WithOne("EquippedSet")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.EquippedSet", "WeaponId");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.Hero", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Characteristics", "Characteristics")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "CharacteristicsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Chronometer", "Chronometer")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "ChronometerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.EquippedSet", "EquippedSet")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "EquippedSetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Health", "Health")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "HealthId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Inventory", "Inventory")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Level", "Level")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "LevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.ResourcePouch", "ResourcePouch")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "ResourcePouchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Statistics", "Statistics")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "StatisticsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.ApplicationUser", "User")
                        .WithOne("Hero")
                        .HasForeignKey("FarmHeroes.Data.Models.HeroModels.Hero", "UserId");
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.HeroModels.HeroEquipment", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Inventory", "Inventory")
                        .WithMany("Storage")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.MappingModels.HeroFight", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.FightModels.Fight", "Fight")
                        .WithMany("HeroFights")
                        .HasForeignKey("FightId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Hero", "Hero")
                        .WithMany("HeroFights")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("FarmHeroes.Data.Models.NotificationModels.HeroModels.Notification", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.HeroModels.Hero", "Hero")
                        .WithMany("Notifications")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.ApplicationUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.ApplicationUser", null)
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FarmHeroes.Data.Models.ApplicationUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FarmHeroes.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
