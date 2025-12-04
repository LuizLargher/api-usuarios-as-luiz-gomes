namespace APIUsuarios.Application.Services;

using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;
using APIUsuarios.Infrastructure.Persistence;
using Application.Dto;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        var usuarios = await _repo.GetAllAsync(ct);
        return usuarios.Select(u => MapToReadDto(u));
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Erro! O ID de usuário deve ser maior que zero.", nameof(id));
        }

        var usuario = await _repo.GetByIdAsync(id, ct);

        if (usuario == null)
        {
            return null;
        }

        var entityDto = MappingExtensions.ToReadDto(usuario);

        return entityDto;
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto), "O corpo da requisição não pode ser vazio.");

        if (await EmailJaCadastradoAsync(dto.Email, ct))
            throw new ArgumentException("Email já cadastrado.", nameof(dto.Email));

        var entity = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            DataCriacao = DateTime.UtcNow
        };

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return MappingExtensions.ToReadDto(entity);
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Erro! O ID de usuário deve ser maior que zero.", nameof(id));
        }

        var usuario = await _repo.GetByIdAsync(id, ct);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Erro! Usuário não encontrado.");
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = dto.Telefone;
        usuario.Ativo = dto.Ativo;
        usuario.DataAtualizacao = DateTime.Now;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        var entityDto = MappingExtensions.ToReadDto(usuario);

        return entityDto;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await _repo.EmailExistsAsync(email, ct);
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct)
    {
        return await _repo.EmailExistsAsync(email, ct);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Erro! O ID de usuário deve ser maior que zero.", nameof(id));
        }

        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null)
        {
            return false;
        }

        await _repo.RemoveAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return true;
    }

    private static UsuarioReadDto MapToReadDto(Usuario u)
    {
        return new UsuarioReadDto(u.Id, u.Nome, u.Email, u.DataNascimento, u.Telefone, u.Ativo, u.DataCriacao);
    }
}