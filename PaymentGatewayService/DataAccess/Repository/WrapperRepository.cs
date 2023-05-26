using DatabaseWrapper;
using PaymentGatewayService.DataAccess.Repository.IRepository;
using PaymentGatewayService.Helpers;
using PaymentGatewayService.Models;

namespace PaymentGatewayService.DataAccess.Repository
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly DatabaseWrapper.DatabaseWrapper _databaseWrapper;

        public WrapperRepository(string connectionString)
        {
            _databaseWrapper = new DatabaseWrapper.DatabaseWrapper(connectionString);
        }

        public async Task InsertBankPayment(PaymentBanks paymentBanks)
        {

             await _databaseWrapper.InsertBankPayment(PBModelConverter.ConvertToPGS(paymentBanks));
        }

        public async Task UpdateBankPayment(PaymentBanks paymentBanks)
        {
            await _databaseWrapper.UpdateBankPayment(PBModelConverter.ConvertToPGS(paymentBanks));
        }

        public async Task DeleteBank(string bank)
        {
            await _databaseWrapper.DeleteBank(bank);
        }

        public async Task<PaymentBanks> GetBankById(Guid bankId)
        {
            var result = await _databaseWrapper.GetBankById(bankId);
             return PBModelConverter.ConvertToDW(result);
        }

        public async Task<List<PaymentBanks>> GetAllBank()
        {
            var result = await _databaseWrapper.GetAllBank();
            return PBModelConverter.ConvertPGSToList(result);
        }
    }
}
