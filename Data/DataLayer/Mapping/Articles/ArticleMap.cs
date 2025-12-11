using DomainLayer.Models.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mapping.Articles;

public class ArticleMap : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles","dbo");
        builder.HasKey(c => c.Id);
        builder.HasIndex(b => b.Url).IsUnique();

        builder.Property(a => a.Body)
            .IsRequired();

        builder.Property(a => a.CreationDate)
            .HasDefaultValueSql("getDate()");

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(350);

        builder.Property(a => a.Url)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(450);

        builder.Property(a => a.MetaDescription)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(a => a.ImageName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(a => a.ShortLink)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(a => a.Tags)
            .IsRequired()
            .HasMaxLength(1000);


        builder.HasOne(a => a.User)
            .WithMany(a => a.Articles)
            .HasForeignKey(a => a.UserId);

        builder.HasOne(a => a.MainGroup)
            .WithMany(a => a.ArticlesMainGroups)
            .HasForeignKey(a => a.GroupId);

        builder.HasOne(a => a.ParentGroup)
            .WithMany(a => a.ArticlesParentGroups)
            .HasForeignKey(a => a.ParentGroupId);
    }
}
