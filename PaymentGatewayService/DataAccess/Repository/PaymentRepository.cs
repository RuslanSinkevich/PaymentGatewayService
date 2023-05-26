using Microsoft.EntityFrameworkCore;
using PaymentGatewayService.DataAccess.Repository.IRepository;
using PaymentGatewayService.Models;

namespace PaymentGatewayService.DataAccess.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentBanks>> ReadAllAsync()
        {
            var bankAmount = await _context.PaymentBanks!.ToListAsync();
            return bankAmount;
        }

        public async Task<PaymentBanks> ReadAsync(Guid id)
        {
            var bankAmount = await _context.PaymentBanks!.FirstOrDefaultAsync(u => u.Id == id);
            if (bankAmount == null) throw new Exception($"class=(PaymentRepository) method=(ReadAsync) - id: ({id}) not found");
            return bankAmount;
        }

        public async Task<PaymentBanks> ReadAsync(int bank)
        {
            var bankPay = await _context.PaymentBanks!.FirstOrDefaultAsync(u => u.Bank == bank);
            return bankPay!;
        }

        public async Task CreateAsync(PaymentBanks? payment)
        {
            await _context.PaymentBanks!.AddAsync(payment!);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PaymentBanks payment)
        {
            _context.PaymentBanks!.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var bankAmount = await _context.PaymentBanks!.FirstOrDefaultAsync(u => u.Id == id);
            if (bankAmount == null) throw new Exception($"class=(PaymentRepository) method=(DeleteAsync) - id: ({id}) not found");
            _context.Remove(bankAmount);
            await _context.SaveChangesAsync();
        }
    }
}
