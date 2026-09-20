using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Entities;

namespace RaizesNordeste.Infrastructure.Persistence;

public class RaizesNordesteDbContext(DbContextOptions<RaizesNordesteDbContext> options)
    : DbContext(options)
{
    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<Cliente> Cliente => Set<Cliente>();
    public DbSet<Unidade> Unidade => Set<Unidade>();
    public DbSet<Produto> Produto => Set<Produto>();
    public DbSet<CardapioUnidade> CardapioUnidade => Set<CardapioUnidade>();
    public DbSet<Estoque> Estoque => Set<Estoque>();
    public DbSet<MovimentacaoEstoque> MovimentacaoEstoque => Set<MovimentacaoEstoque>();
    public DbSet<Pedido> Pedido => Set<Pedido>();
    public DbSet<ItemPedido> ItemPedido => Set<ItemPedido>();
    public DbSet<Pagamento> Pagamento => Set<Pagamento>();
    public DbSet<ContaFidelidade> ContaFidelidade => Set<ContaFidelidade>();
    public DbSet<MovimentacaoPontos> MovimentacaoPontos => Set<MovimentacaoPontos>();
    public DbSet<ConsentimentoLGPD> ConsentimentoLgpd => Set<ConsentimentoLGPD>();
    public DbSet<Promocao> Promocao => Set<Promocao>();
    public DbSet<Auditoria> Auditoria => Set<Auditoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaizesNordesteDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
