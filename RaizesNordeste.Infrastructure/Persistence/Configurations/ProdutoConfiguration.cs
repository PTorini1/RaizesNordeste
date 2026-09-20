using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produto");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .HasColumnName("Nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Descricao)
            .HasColumnName("Descricao")
            .HasMaxLength(500);

        builder.Property(p => p.PrecoBase)
            .HasColumnName("PrecoBase")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Categoria)
            .HasColumnName("Categoria")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Sazonal)
            .HasColumnName("Sazonal")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(p => p.Ativo)
            .HasColumnName("Ativo")
            .HasDefaultValue(true)
            .IsRequired();
    }
}
