using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.ToTable("MovimentacaoEstoque");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.EstoqueId)
            .HasColumnName("EstoqueId")
            .IsRequired();

        builder.Property(m => m.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();

        builder.Property(m => m.TipoMovimentacao)
            .HasColumnName("TipoMovimentacao")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Quantidade)
            .HasColumnName("Quantidade")
            .IsRequired();

        builder.Property(m => m.Motivo)
            .HasColumnName("Motivo")
            .HasMaxLength(255);

        builder.Property(m => m.CriadoEm)
            .HasColumnName("CriadoEm")
            .IsRequired();

        builder.HasOne(m => m.Estoque)
            .WithMany()
            .HasForeignKey(m => m.EstoqueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Usuario)
            .WithMany()
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
