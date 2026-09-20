using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Cliente");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();

        builder.Property(c => c.Cpf)
            .HasColumnName("Cpf")
            .HasMaxLength(14);

        builder.HasIndex(c => c.Cpf)
            .IsUnique();

        builder.Property(c => c.Telefone)
            .HasColumnName("Telefone")
            .HasMaxLength(20);

        builder.Property(c => c.DataNascimento)
            .HasColumnName("DataNascimento")
            .HasColumnType("date");

        builder.Property(c => c.Ativo)
            .HasColumnName("Ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.HasOne(c => c.Usuario)
            .WithOne()
            .HasForeignKey<Cliente>(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
