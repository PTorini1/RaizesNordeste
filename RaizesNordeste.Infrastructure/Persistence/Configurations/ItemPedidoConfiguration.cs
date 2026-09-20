using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItemPedido");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.PedidoId)
            .HasColumnName("PedidoId")
            .IsRequired();

        builder.Property(i => i.ProdutoId)
            .HasColumnName("ProdutoId")
            .IsRequired();

        builder.Property(i => i.Quantidade)
            .HasColumnName("Quantidade")
            .IsRequired();

        builder.Property(i => i.ValorUnitario)
            .HasColumnName("ValorUnitario")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(i => i.ValorTotal)
            .HasColumnName("ValorTotal")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.HasOne(i => i.Pedido)
            .WithMany(p => p.Itens)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
