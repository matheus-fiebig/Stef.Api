using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using API.STEF.Domain.OrderAggregator;

namespace API.STEF.Data.Context.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.ComplexProperty(x => x.Customer).IsRequired();

            builder.ComplexProperty(x => x.Customer)
                .Property(x => x.Name)
                .HasColumnName("NomeCliente")
                .HasColumnType("varchar")
                .HasMaxLength(80)
                .IsRequired();

            builder.ComplexProperty(x => x.Customer)
                .Property(x => x.Email)
                .HasColumnName("EmailCliente")
                .HasColumnType("varchar")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.Paid)
                .HasColumnName("Pago")
                .HasColumnType("bit")
                .IsRequired();

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
