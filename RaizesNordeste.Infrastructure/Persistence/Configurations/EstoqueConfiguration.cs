using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class EstoqueConfiguration : IEntityTypeConfiguration<Estoque>
{
    public void Configure(EntityTypeBuilder<Estoque> builder)
    {
        builder.ToTable("Estoque");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UnidadeId)
            .HasColumnName("UnidadeId")
            .IsRequired();

        builder.Property(e => e.ProdutoId)
            .HasColumnName("ProdutoId")
            .IsRequired();

        builder.Property(e => e.QuantidadeAtual)
            .HasColumnName("QuantidadeAtual")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.QuantidadeMinima)
            .HasColumnName("QuantidadeMinima")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.AtualizadoEm)
            .HasColumnName("AtualizadoEm")
            .IsRequired();

        builder.HasIndex(e => new { e.UnidadeId, e.ProdutoId })
            .IsUnique();

        builder.HasOne(e => e.Unidade)
            .WithMany()
            .HasForeignKey(e => e.UnidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Produto)
            .WithMany()
            .HasForeignKey(e => e.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
