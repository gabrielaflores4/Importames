using Microsoft.EntityFrameworkCore;
using Importames.Models;

namespace Importames.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<VehiculoModel> Vehiculos { get; set; }
    }
}
