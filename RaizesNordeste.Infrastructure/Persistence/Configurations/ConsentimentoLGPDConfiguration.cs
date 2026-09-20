using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence.Configurations;

public class ConsentimentoLGPDConfiguration : IEntityTypeConfiguration<ConsentimentoLGPD>
{
    public void Configure(EntityTypeBuilder<ConsentimentoLGPD> builder)
    {
        builder.ToTable("ConsentimentoLgpd");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClienteId)
            .HasColumnName("ClienteId")
            .IsRequired();

        builder.Property(c => c.Finalidade)
            .HasColumnName("Finalidade")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Consentido)
            .HasColumnName("Consentido")
            .IsRequired();

        builder.Property(c => c.VersaoTermo)
            .HasColumnName("VersaoTermo")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ConcedidoEm)
            .HasColumnName("ConcedidoEm");

        builder.Property(c => c.RevogadoEm)
            .HasColumnName("RevogadoEm");

        builder.HasOne(c => c.Cliente)
            .WithMany(cl => cl.ConsentimentoLgpd)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
