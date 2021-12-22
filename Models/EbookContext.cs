using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EbookSTR
{
    public partial class EbookContext : DbContext
    {
        public EbookContext()
        {
        }

        public EbookContext(DbContextOptions<EbookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Ebook> Ebooks { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server= DESKTOP-NQ12799\\SQLEXPRESS; Database=Ebook; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.GenreId).HasColumnName("genreId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Authors)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Authors_Genres");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("surname")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ebook>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("authorId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .IsFixedLength(true);

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("format")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Pages)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("pages")
                    .IsFixedLength(true);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("price")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Ebooks)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ebooks_Authors");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.EbookId).HasColumnName("ebookId");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Clients");

                entity.HasOne(d => d.Ebook)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.EbookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Ebooks");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
