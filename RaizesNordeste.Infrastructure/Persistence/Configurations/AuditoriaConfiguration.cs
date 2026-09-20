using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("Auditoria");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.UsuarioId)
            .HasColumnName("UsuarioId");

        builder.Property(a => a.Entidade)
            .HasColumnName("Entidade")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.EntidadeId)
            .HasColumnName("EntidadeId")
            .IsRequired();

        builder.Property(a => a.Acao)
            .HasColumnName("Acao")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.DadosAnteriores)
            .HasColumnName("DadosAnteriores")
            .HasColumnType("text");

        builder.Property(a => a.DadosNovos)
            .HasColumnName("DadosNovos")
            .HasColumnType("text");

        builder.Property(a => a.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
