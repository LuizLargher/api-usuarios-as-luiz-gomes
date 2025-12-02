namespace APIUsuarios.Application.Services;

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
        return usuarios.Select(u => u.ToReadDto());
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

        var produtoDTO = MappingExtensions.ToReadDto(usuario);

        return produtoDTO;
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto user, CancellationToken ct = default)
    {
        if (await EmailJaCadastradoAsync(user.Email, ct))
            throw new Exception("E-mail já cadastrado.");

        var usuario = new Usuario
        {
            Nome = user.Nome,
            Email = user.Email,
            Senha = user.Senha,
            DataCriacao = DateTime.UtcNow
        };

        await _context.Usuario.AddAsync(usuario, ct);
        await _context.SaveChangesAsync(ct);

        return new UsuarioReadDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            DataCriacao = usuario.DataCriacao
        };
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

        return usuario.ToReadDto();
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
}