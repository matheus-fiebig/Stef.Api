using API.STEF.Domain.OrderAggregator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.STEF.Data.Context.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("ItensPedido");

            builder.HasKey(x => new { x.Id, x.ProductId, x.OrderId });

            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .HasColumnName("Id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.OrderId)
                .HasColumnName("IdPedido")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.ProductId)
                .HasColumnName("IdProduto")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName("Quantidade")
                .HasColumnType("int")
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);
        }
    }
}
