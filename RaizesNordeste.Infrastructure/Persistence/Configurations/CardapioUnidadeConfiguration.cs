using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class CardapioUnidadeConfiguration : IEntityTypeConfiguration<CardapioUnidade>
{
    public void Configure(EntityTypeBuilder<CardapioUnidade> builder)
    {
        builder.ToTable("CardapioUnidade");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UnidadeId)
            .HasColumnName("UnidadeId")
            .IsRequired();

        builder.Property(c => c.ProdutoId)
            .HasColumnName("ProdutoId")
            .IsRequired();

        builder.Property(c => c.Preco)
            .HasColumnName("Preco")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(c => c.Disponivel)
            .HasColumnName("Disponivel")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.InicioVigencia)
            .HasColumnName("InicioVigencia");

        builder.Property(c => c.FimVigencia)
            .HasColumnName("FimVigencia");

        builder.HasIndex(c => new { c.UnidadeId, c.ProdutoId })
            .IsUnique();

        builder.HasOne(c => c.Unidade)
            .WithMany()
            .HasForeignKey(c => c.UnidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Produto)
            .WithMany()
            .HasForeignKey(c => c.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
