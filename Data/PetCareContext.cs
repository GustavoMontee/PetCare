using Microsoft.EntityFrameworkCore;
using PetCare.Models;

namespace PetCare.Data
{
    public class PetCareContext : DbContext
    {
        public PetCareContext(DbContextOptions<PetCareContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<UsuarioSistema> UsuariosSistema { get; set; }

        public DbSet<Pet> Pets { get; set; }
    }
}