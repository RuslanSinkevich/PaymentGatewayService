using PaymentGatewayService.Models;


namespace PaymentGatewayService.DataAccess.Repository.IRepository
{
    public interface IWrapperRepository
    {
        Task InsertBankPayment(PaymentBanks paymentBanks);
        Task UpdateBankPayment(PaymentBanks paymentBanks);
        Task DeleteBank(string bank);
        Task<PaymentBanks> GetBankById(Guid bankId);
        Task<List<PaymentBanks>> GetAllBank();
    }
}
