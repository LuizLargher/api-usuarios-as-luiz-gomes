namespace APIUsuarios.Infrastructure.Persistence;
using APIUsuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuario { get; set; }
}