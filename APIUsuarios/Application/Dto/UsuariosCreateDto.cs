namespace APIUsuarios.Application.Dto;

public record UsuarioCreateDto(

string Nome,

string Email,

string Senha,

DateTime DataNascimento,

string? Telefone

);
