using RaizesNordeste.Domain.Entities;
using RaizesNordeste.Domain.Enums;
using RaizesNordeste.Domain.Exceptions;
using RaizesNordeste.Domain.Validators;

namespace RaizesNordeste.Domain.Services;

public class UsuarioDomainService
{
    public void ValidarEmailDisponivel(IEnumerable<Usuario> existentes, string email)
    {
        if (existentes.Any())
        {
            throw new DomainException("Já existe um usuário cadastrado com este e-mail.");
        }
    }

    public void ValidarAtivo(Usuario? usuario)
    {
        if (usuario is null || !usuario.Ativo)
        {
            throw new DomainException("Usuário inválido ou inativo.");
        }
    }

    public Usuario CriarUsuario(string nome, string email, string firebaseUid, RoleUsuario role, bool ativo)
    {
        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            FirebaseUid = firebaseUid,
            Role = role,
            Ativo = ativo,
            CriadoEm = DateTime.UtcNow
        };

        new UsuarioValidator().ValidaOuLancaExcecao(usuario);
        return usuario;
    }

    public Usuario ResolverAutoRegistro(string email, string firebaseUid)
    {
        var nomeExibicao = email.Split('@')[0];
        var nomeFinal = char.ToUpper(nomeExibicao[0]) + nomeExibicao[1..];

        var usuario = new Usuario
        {
            Nome = nomeFinal,
            Email = email,
            FirebaseUid = firebaseUid,
            Role = RoleUsuario.Cliente,
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        new UsuarioValidator().ValidaOuLancaExcecao(usuario);
        return usuario;
    }

    public Cliente CriarClienteParaUsuario(int usuarioId)
    {
        var cliente = new Cliente
        {
            UsuarioId = usuarioId,
            Cpf = string.Empty,
            Telefone = string.Empty,
            DataNascimento = DateTime.MinValue,
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        new ClienteValidator().ValidaOuLancaExcecao(cliente);
        return cliente;
    }

    public void AlterarRole(Usuario usuario, RoleUsuario novaRole)
    {
        usuario.Role = novaRole;
        new UsuarioValidator().ValidaOuLancaExcecao(usuario);
    }
}
