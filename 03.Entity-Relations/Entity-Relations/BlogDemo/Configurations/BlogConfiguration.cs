using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogDemo.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {

        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder
                .HasKey(b => b.BlogId);
            //.HasKey("BlogId");

            builder
                 .ToTable("Blogs", "blg"); // даваме име на таблицата и схемата

            builder
                .Property(b => b.Name)
                .HasColumnName("BlogName")   //custom column name
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
            .IsRequired();

            builder
                 .Property(b => b.Description)
                .HasColumnType("NVARCHAR")
            .HasMaxLength(500);

            builder
                .Property(b => b.LastUpdated)
            .ValueGeneratedOnUpdate();

            builder
                .Property(b => b.Created)
                .ValueGeneratedOnAdd();


        }
    }
}
