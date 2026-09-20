using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class ContaFidelidadeConfiguration : IEntityTypeConfiguration<ContaFidelidade>
{
    public void Configure(EntityTypeBuilder<ContaFidelidade> builder)
    {
        builder.ToTable("ContaFidelidade");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClienteId)
            .HasColumnName("ClienteId")
            .IsRequired();

        builder.Property(c => c.SaldoPontos)
            .HasColumnName("SaldoPontos")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(c => c.Ativa)
            .HasColumnName("Ativa")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.HasIndex(c => c.ClienteId)
            .IsUnique();

        builder.HasOne(c => c.Cliente)
            .WithOne(cl => cl.ContaFidelidade)
            .HasForeignKey<ContaFidelidade>(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
