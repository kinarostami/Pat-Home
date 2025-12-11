using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Users
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        //IsRequired Default Value = True
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "dbo");
            entity.HasIndex(u => u.PhoneNumber).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasKey(u => u.Id);


            entity.Property(b => b.Gender)
                .HasDefaultValue(UserGender.نا_مشخص);

            entity.Property(u => u.Name)
                .IsRequired(false)
                .HasMaxLength(50);

            entity.Property(u => u.Family)
               .IsRequired(false)
               .HasMaxLength(50);

            entity.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            entity.Property(u => u.ImageName)
                .IsRequired(false)
                .HasDefaultValue("Default.jpg")
                .HasMaxLength(150);

            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(100);

            entity.Property(u => u.PhoneNumber)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(11);

            entity.Property(u => u.Presenter)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(11);

            entity.Property(u => u.NationalCode)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(10);

            entity.Property(c => c.IsActive);

            entity.Property(u => u.ActiveCode)
                .HasDefaultValueSql("NEWID()")
                .HasMaxLength(150);

            entity.Property(u => u.SecondActiveCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(5);

        }
    }
}
