using EquipmentLoanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentLoanApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definindo a chave primária para Users
            modelBuilder.Entity<User>().HasKey(u => u.Cpf);

            // Configurando relações:
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.Cpf)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Microfone", Quantity = 12 },
            new Product { Id = 2, Name = "Datashow", Quantity = 4 },
            new Product { Id = 3, Name = "Televisor", Quantity = 2 },
            new Product { Id = 4, Name = "Aparelho de DVD", Quantity = 7 },
            new Product { Id = 5, Name = "Caixa de Som", Quantity = 15 },
            new Product { Id = 6, Name = "Projetor", Quantity = 3 },
            new Product { Id = 7, Name = "Sistema de Áudio", Quantity = 2 },
            new Product { Id = 8, Name = "Equipamento de Streaming", Quantity = 6 },
            new Product { Id = 9, Name = "Gravador Digital", Quantity = 8 }
            );
        }
    }
}
