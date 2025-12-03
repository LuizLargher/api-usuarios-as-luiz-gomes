namespace APIUsuarios.Application.Services;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;
using APIUsuarios.Infrastructure.Persistence;
using Application.Dto;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly AppDbContext _context;

    public UsuarioService(IUsuarioRepository repo, AppDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct = default)
    {
        var usuarios = await _repo.GetAllAsync(ct);
        return usuarios.Select(u => MapToReadDto(u));
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct = default)
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

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto user, CancellationToken ct = default)
    {
        if (await EmailJaCadastradoAsync(user.Email, ct))
            throw new Exception("Email já cadastrado.");

        var entity = new Usuario
        {
            Nome = user.Nome,
            Email = user.Email,
            DataCriacao = DateTime.UtcNow
        };

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        var entityDto = MappingExtensions.ToReadDto(entity);

        return entityDto;
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct = default)
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

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        return await _repo.EmailExistsAsync(email, ct);
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct = default)
    {
        return await _repo.EmailExistsAsync(email, ct);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
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