using PaymentGatewayService.Models;

namespace PaymentGatewayService.Services.IService
{
    public interface IPaymentService
    {
        Task<List<PaymentBanks>> ReadAllAsync();
        Task<PaymentBanks> ReadAsync(Guid id);
        Task<PaymentBanks> CreateAsync(PaymentBanks paymentVm);
        Task UpdateAsync(PaymentBanks payment);
        Task DeleteAsync(Guid id);
    }
}
