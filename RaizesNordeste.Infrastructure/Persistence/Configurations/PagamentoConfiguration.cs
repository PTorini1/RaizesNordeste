using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamento");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.PedidoId)
            .HasColumnName("PedidoId")
            .IsRequired();

        builder.Property(p => p.Provedor)
            .HasColumnName("Provedor")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.TransacaoExternaId)
            .HasColumnName("TransacaoExternaId")
            .HasMaxLength(150);

        builder.Property(p => p.PayloadEnvio)
            .HasColumnName("PayloadEnvio")
            .HasColumnType("text");

        builder.Property(p => p.PayloadRetorno)
            .HasColumnName("PayloadRetorno")
            .HasColumnType("text");

        builder.Property(p => p.ChaveIdempotencia)
            .HasColumnName("ChaveIdempotencia")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.SolicitadoEm)
            .HasColumnName("SolicitadoEm")
            .IsRequired();

        builder.Property(p => p.RespondidoEm)
            .HasColumnName("RespondidoEm");

        builder.HasIndex(p => p.PedidoId)
            .IsUnique();

        builder.HasIndex(p => p.ChaveIdempotencia)
            .IsUnique();

        builder.HasOne(p => p.Pedido)
            .WithOne(pe => pe.Pagamento)
            .HasForeignKey<Pagamento>(p => p.PedidoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
