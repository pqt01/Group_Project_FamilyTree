﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
	public class FUFamilyTreeContext : DbContext
	{
		#region DbSet
		//public DbSet<User> Users { get; set; }
		#endregion
		//public FUFamilyTreeContext(DbContextOptions options) : base(options) { }
		public string GetConnectionString()
		{
			IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
			var strConn = config["ConnectionStrings:DB"];
			return strConn;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//if (!optionsBuilder.IsConfigured)
			//{
			optionsBuilder.UseSqlServer("server =(local);database=FUFamilyTree;uid=sa;pwd=12345;TrustServerCertificate=True");
			//		.LogTo(s => System.Diagnostics.Debug.WriteLine(s))
			//			 .EnableDetailedErrors()
			//			 .EnableSensitiveDataLogging();
			//	optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			//}
		}
		override protected void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Account>(entity =>
			{
				entity.ToTable("Account").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.Property(e => e.Username)
					.IsRequired()
					.HasMaxLength(50)
					.IsUnicode(false);
				entity.Property(e => e.Password)
					.IsRequired()
					.HasMaxLength(50);
				entity.Property(e => e.Email)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);
				entity.Property(e => e.PhoneNumber)
					.HasMaxLength(20)
					.IsUnicode(false);
				entity.Property(e => e.Role)
					.IsRequired()
					.HasMaxLength(10)
					.HasDefaultValue("MEMBER")
					.IsUnicode(false);
				entity.HasIndex(e => e.Username).IsUnique(true);
				entity.HasIndex(e => e.Email).IsUnique(true);
				entity.HasIndex(e => e.PhoneNumber).IsUnique(true);
			});

			modelBuilder.Entity<Member>(entity =>
			{
				entity.ToTable("Member").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.Property(e => e.FullName)
					.IsRequired()
					.HasMaxLength(100);
				entity.Property(e => e.LinkAvatar)
					.IsRequired()
					.HasMaxLength(300);
				entity.Property(e => e.Gender)
					.IsRequired()
					.HasDefaultValue(true);
				entity.HasOne(e => e.Account)
					.WithOne(e => e.Member)
					.HasForeignKey<Member>(e => e.AccountId)
					.HasConstraintName("FK_Member_Account");
				entity.HasOne(e => e.Parent)
					.WithMany(e => e.Childs)
					.HasForeignKey(e => e.ParentId)
					.HasConstraintName("FK_Parent_Member_Couple");
				entity.HasOne(e => e.Family)
					.WithMany(e => e.Members)
					.HasForeignKey(e => e.FamilyId)
					.HasConstraintName("FK_Member_Family");
			});
			modelBuilder.Entity<Couple>(entity =>
			{
				entity.ToTable("Couple").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.HasOne(e => e.Father)
					.WithMany(e => e.CouplesFather)
					.HasForeignKey(e => e.FaId)
					.HasConstraintName("FK_Father_Couple_Member");
				entity.HasOne(e => e.Mother)
					.WithMany(e => e.CouplesMother)
					.HasForeignKey(e => e.MoId)
					.HasConstraintName("FK_Mother_Couple_Member");
			});
			modelBuilder.Entity<Family>(entity =>
			{
				entity.ToTable("Family").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.HasOne(e => e.Creator)
					.WithOne(e => e.FamilyCreated)
					.HasForeignKey<Family>(e => e.CreatorId)
					.HasConstraintName("FK_Creator_Family_Member");
			});
			modelBuilder.Entity<Image>(entity =>
			{
				entity.ToTable("Image").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.Property(e => e.Url)
					.IsRequired()
					.HasMaxLength(400);
				entity.HasOne(e => e.Family)
					.WithMany(e => e.Images)
					.HasForeignKey(e => e.FamilyId)
					.HasConstraintName("FK_Image_Family");
			});
			modelBuilder.Entity<Event>(entity =>
			{
				entity.ToTable("Event").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.HasOne(e => e.Family)
					.WithMany(e => e.Events)
					.HasForeignKey(e => e.FamilyId)
					.HasConstraintName("FK_Event_Family");
				entity.HasOne(e => e.Service)
					.WithMany(e => e.Events)
					.HasForeignKey(e => e.ServiceId)
					.HasConstraintName("FK_Event_Service");
				entity.HasOne(e => e.Location)
					.WithMany(e => e.Events)
					.HasForeignKey(e => e.LocationId)
					.HasConstraintName("FK_Event_Location");
			});
			modelBuilder.Entity<Service>(entity =>
			{
				entity.ToTable("Service").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);
				entity.Property(e => e.Price)
					.IsRequired()
					.HasDefaultValue(0);
				entity.Property(e => e.Rating)
					.IsRequired()
					.HasDefaultValue(0);
			});
			modelBuilder.Entity<Location>(entity =>
			{
				entity.ToTable("Location").HasKey(e => e.Id);
				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd();
				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);
				entity.Property(e => e.Price)
					.IsRequired()
					.HasDefaultValue(0);
				entity.Property(e => e.Rating)
					.IsRequired()
					.HasDefaultValue(0);
			});

		}
	}
}