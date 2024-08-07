using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApp.Entities;

namespace ShopApp.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(c => c.IsDelete).HasDefaultValue(false);
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(c => c.UpdatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
