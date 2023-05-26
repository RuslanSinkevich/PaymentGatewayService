using Microsoft.EntityFrameworkCore;
using PaymentGatewayService.Models;

namespace PaymentGatewayService.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<PaymentBanks>? PaymentBanks { get; set; }
    }
}
