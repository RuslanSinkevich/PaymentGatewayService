using PaymentGatewayService.Enums;
using PaymentGatewayService.Models;

namespace PaymentGatewayService.DataAccess.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task<List<PaymentBanks>> ReadAllAsync();
        Task<PaymentBanks> ReadAsync(Guid id);
        Task<PaymentBanks> ReadAsync(int bank);
        Task CreateAsync(PaymentBanks? payment);
        Task UpdateAsync(PaymentBanks payment);
        Task DeleteAsync(Guid id);
    }
}
