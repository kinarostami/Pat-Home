using DomainLayer.Models.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Articles
{
    public class ArticleGroupMap : IEntityTypeConfiguration<ArticleGroup>
    {
        public void Configure(EntityTypeBuilder<ArticleGroup> builder)
        {
            builder.ToTable("ArticleGroups", "dbo");
            builder.HasKey(c => c.Id);
            builder.HasIndex(b => b.EnglishTitle).IsUnique();

            builder.Property(g => g.GroupTitle)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.EnglishTitle)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(300);

            builder.Property(g => g.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}