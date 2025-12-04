namespace APIUsuarios.Infrastructure.Repositories;
using APIUsuarios.Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext _context)
    {
        this._context = _context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Usuario.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Usuario.FindAsync(new object[] { id }, ct);
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Usuario.FindAsync(new object[] { email }, ct);
    }

    public async Task AddAsync(Usuario usuario, CancellationToken ct = default)
    {
        await _context.Usuario.AddAsync(usuario, ct);
    }

    public Task UpdateAsync(Usuario usuario, CancellationToken ct = default)
    {
        _context.Usuario.Update(usuario);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Usuario usuario, CancellationToken ct = default)
    {
        usuario.Ativo = false;
        return Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _context.Usuario
            .AnyAsync(u => u.Email == email, ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        try
        {
            return await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Erro! Houve um problema ao salvar mudanças no banco de dados", ex);
        }
    }
}