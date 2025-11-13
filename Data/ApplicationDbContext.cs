using DiarioMagna.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiarioMagna.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Noticiero> Noticieros { get; set; }
        public DbSet<ArticleNoticiero> ArticleNoticieros { get; set; }
        public DbSet<ArticleReference> ArticleReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.AuthorName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.AuthorEmail).HasMaxLength(200).IsRequired();
                entity.Property(e => e.AuthorPhone).HasMaxLength(30);
                entity.Property(e => e.Status).HasConversion<int>();
            });

            builder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            builder.Entity<Plan>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Noticiero>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.ContactEmail).HasMaxLength(200);
                entity.Property(e => e.ContactPhone).HasMaxLength(30);
            });

            builder.Entity<ArticleReference>(entity =>
            {
                entity.HasOne(ar => ar.Article)
                    .WithMany(a => a.References)
                    .HasForeignKey(ar => ar.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.ReferencedArticle)
                    .WithMany()
                    .HasForeignKey(ar => ar.ReferencedArticleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ArticleNoticiero>(entity =>
            {
                entity.HasOne(an => an.Article)
                    .WithMany(a => a.ArticleNoticieros)
                    .HasForeignKey(an => an.ArticleId);

                entity.HasOne(an => an.Noticiero)
                    .WithMany(n => n.ArticleNoticieros)
                    .HasForeignKey(an => an.NoticieroId);
            });
        }
    }
}
