namespace APIUsuarios.Application.Services;
using Application.Dto;
using Domain.Entities;

public static class MappingExtensions
{
    public static UsuarioReadDto? ToReadDto(this Usuario user)
    {
        if (user == null) return null;

        return new UsuarioReadDto(
            Id: user.Id,
            Nome: user.Nome,
            Email: user.Email,
            DataNascimento: user.DataNascimento,
            Telefone: user.Telefone,
            Ativo: user.Ativo,
            DataCriacao: user.DataCriacao
        );
    }
}