using API.STEF.Domain.ProductAggregator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.STEF.Data.Context.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(20)
                .HasColumnName("NomeProduto")
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(x => x.Value)
                .HasColumnName("Valor")
                .HasColumnType("decimal(10,2)")
                .IsRequired();
        }
    }
}
