using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class UnidadeConfiguration : IEntityTypeConfiguration<Unidade>
{
    public void Configure(EntityTypeBuilder<Unidade> builder)
    {
        builder.ToTable("Unidade");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Nome)
            .HasColumnName("Nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Cidade)
            .HasColumnName("Cidade")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Estado)
            .HasColumnName("Estado")
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.Property(u => u.Endereco)
            .HasColumnName("Endereco")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.TipoOperacao)
            .HasColumnName("TipoOperacao")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Ativa)
            .HasColumnName("Ativa")
            .HasDefaultValue(true)
            .IsRequired();
    }
}
