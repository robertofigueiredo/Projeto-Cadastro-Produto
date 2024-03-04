using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto_API_Conceitos.Models;

namespace Projeto_API_Conceitos.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base (options)
        {

        }

        public DbSet<Produto> produtos { get; set; }
    }
}
