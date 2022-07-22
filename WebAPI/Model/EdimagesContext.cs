using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPI.Model
{
    public partial class EdimagesContext : DbContext
    {
        public EdimagesContext()
        {
        }

        public EdimagesContext(DbContextOptions<EdimagesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Imagen> Imagens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SNCCH01LABF120\\SQLEXPRESS;Initial Catalog=Edimages;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Imagen>(entity =>
            {
                entity.ToTable("imagens");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bytes)
                    .HasColumnType("image")
                    .HasColumnName("bytes");

                entity.Property(e => e.Title)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Uri)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("uri");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
