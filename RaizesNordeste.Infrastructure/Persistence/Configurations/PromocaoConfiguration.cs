using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class PromocaoConfiguration : IEntityTypeConfiguration<Promocao>
{
    public void Configure(EntityTypeBuilder<Promocao> builder)
    {
        builder.ToTable("Promocao");

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

        builder.Property(p => p.TipoDesconto)
            .HasColumnName("TipoDesconto")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.ValorDesconto)
            .HasColumnName("ValorDesconto")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.InicioVigencia)
            .HasColumnName("InicioVigencia")
            .IsRequired();

        builder.Property(p => p.FimVigencia)
            .HasColumnName("FimVigencia")
            .IsRequired();

        builder.Property(p => p.Ativa)
            .HasColumnName("Ativa")
            .HasDefaultValue(true)
            .IsRequired();
    }
}
