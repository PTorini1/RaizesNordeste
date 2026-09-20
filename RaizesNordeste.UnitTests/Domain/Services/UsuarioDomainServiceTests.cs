using FluentAssertions;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Services;
using RaizesNordeste.UnitTests.Helpers;

namespace RaizesNordeste.UnitTests.Domain.Services;

public class UsuarioDomainServiceTests
{
    private readonly UsuarioDomainService _service = new();

    [Fact]
    public void ValidarEmailDisponivel_DeveLancarDomainException_QuandoEmailJaExistir()
    {
        var existentes = new[] { Builders.Usuario(email: "admin@teste.com") };

        var act = () => _service.ValidarEmailDisponivel(existentes, "admin@teste.com");

        act.Should().Throw<DomainException>()
            .WithMessage("Já existe um usuário cadastrado com este e-mail.");
    }

    [Fact]
    public void CriarUsuario_DeveCriarUsuario_QuandoDadosForemValidos()
    {
        var usuario = _service.CriarUsuario("Admin", "admin@teste.com", "firebase-uid", RoleUsuario.Admin, true);

        usuario.Role.Should().Be(RoleUsuario.Admin);
        usuario.Ativo.Should().BeTrue();
    }

    [Fact]
    public void CriarUsuario_DeveLancarDomainException_QuandoEmailForInvalido()
    {
        var act = () => _service.CriarUsuario("Admin", "email-invalido", "firebase-uid", RoleUsuario.Admin, true);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ResolverAutoRegistro_DeveCriarUsuarioClienteAPartirDoEmail()
    {
        var usuario = _service.ResolverAutoRegistro("maria@teste.com", "uid-1");

        usuario.Nome.Should().Be("Maria");
        usuario.Role.Should().Be(RoleUsuario.Cliente);
        usuario.Ativo.Should().BeTrue();
    }

    [Fact]
    public void CriarClienteParaUsuario_DeveLancarDomainException_QuandoUsuarioIdForInvalido()
    {
        var act = () => _service.CriarClienteParaUsuario(0);

        act.Should().Throw<DomainException>()
            .WithMessage("*Usuário inválido*");
    }

    [Fact]
    public void AlterarRole_DeveAtualizarRoleDoUsuario()
    {
        var usuario = Builders.Usuario(role: RoleUsuario.Atendente);

        _service.AlterarRole(usuario, RoleUsuario.Gerente);

        usuario.Role.Should().Be(RoleUsuario.Gerente);
    }
}
