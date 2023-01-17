using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StoreFunctions
{
  public partial class TrainingAlinContext : DbContext
  {
    public TrainingAlinContext()
    {
    }

    public TrainingAlinContext(DbContextOptions<TrainingAlinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable($"ConnectionStrings:database", EnvironmentVariableTarget.Process));
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Product>(entity =>
      {
        entity.Property(e => e.Id)
                  .ValueGeneratedNever()
                  .HasColumnName("id");

        entity.Property(e => e.Description)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false)
                  .HasColumnName("description");

        entity.Property(e => e.Image)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false)
                  .HasColumnName("image");

        entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("name");

        entity.Property(e => e.Price).HasColumnName("price");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Email)
                  .HasName("PK_users");

        entity.Property(e => e.Email)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("email");

        entity.Property(e => e.FullName)
              .HasMaxLength(100)
              .IsUnicode(false)
              .HasColumnName("fullname");

        entity.Property(e => e.Address)
              .IsRequired()
              .HasMaxLength(500)
              .IsUnicode(false)
              .HasColumnName("address");

        entity.Property(e => e.HashPass)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .HasColumnName("hashpass");

        entity.Property(e => e.Phone)
                  .IsRequired()
                  .HasMaxLength(12)
                  .IsUnicode(false)
                  .HasColumnName("phone");

        entity.Property(e => e.Role).HasColumnName("role");

        entity.Property(e => e.HashSalt)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("hashSalt");

        entity.Property(e => e.RefreshToken)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("refreshToken");
      });

      OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
