using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class MovimentacaoPontosConfiguration : IEntityTypeConfiguration<MovimentacaoPontos>
{
    public void Configure(EntityTypeBuilder<MovimentacaoPontos> builder)
    {
        builder.ToTable("MovimentacaoPontos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.ContaFidelidadeId)
            .HasColumnName("ContaFidelidadeId")
            .IsRequired();

        builder.Property(m => m.PedidoId)
            .HasColumnName("PedidoId");

        builder.Property(m => m.TipoMovimentacao)
            .HasColumnName("TipoMovimentacao")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Pontos)
            .HasColumnName("Pontos")
            .IsRequired();

        builder.Property(m => m.Descricao)
            .HasColumnName("Descricao")
            .HasMaxLength(255);

        builder.Property(m => m.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.HasOne(m => m.ContaFidelidade)
            .WithMany()
            .HasForeignKey(m => m.ContaFidelidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Pedido)
            .WithMany()
            .HasForeignKey(m => m.PedidoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
