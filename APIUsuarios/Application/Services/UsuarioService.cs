namespace APIUsuarios.Application.Services;

using Application.Dto;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct = default)
    {
        return await _context.Usuario
        .Select(u => new UsuarioReadDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            DataNascimento = u.DataNascimento,
            Telefone = u.Telefone,
            Ativo = u.Ativo,
            DataCriacao = u.DataCriacao
        })
        .ToListAsync(ct);
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

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct = default)
    {
        return await ;
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct = default)
    {
        return await ;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        return await ;
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct = default)
    {
        return await ;
    }

    public Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}