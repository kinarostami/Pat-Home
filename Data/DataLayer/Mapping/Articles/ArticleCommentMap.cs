using DomainLayer.Models.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Articles
{
    public class ArticleCommentMap: IEntityTypeConfiguration<ArticleComment>
    {
        public void Configure(EntityTypeBuilder<ArticleComment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("ArticleComments","dbo");


            builder.Property(a => a.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("getDate()");


            builder.HasOne(c => c.Article)
                .WithMany(a => a.ArticleComments)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}