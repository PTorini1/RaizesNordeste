using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedido");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.ClienteId)
            .HasColumnName("ClienteId");

        builder.Property(p => p.UnidadeId)
            .HasColumnName("UnidadeId")
            .IsRequired();

        builder.Property(p => p.CanalPedido)
            .HasColumnName("CanalPedido")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.ValorTotal)
            .HasColumnName("ValorTotal")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.Property(p => p.AtualizadoEm)
            .HasColumnName("AtualizadoEm");

        builder.HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Unidade)
            .WithMany()
            .HasForeignKey(p => p.UnidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Itens)
            .WithOne(i => i.Pedido)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
