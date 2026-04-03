using Microsoft.EntityFrameworkCore;
using Importames.Models;

namespace Importames.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<VehiculoModel> Vehiculos { get; set; }
        public DbSet<EstadosModel> Estados { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<HistorialEstadoModel> Historiales { get; set; }
    }
}
