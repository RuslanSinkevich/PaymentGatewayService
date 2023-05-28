using PaymentGatewayService.DataAccess.Repository.IRepository;
using PaymentGatewayService.Enums;
using PaymentGatewayService.Helpers;
using PaymentGatewayService.Models;
using PaymentGatewayService.Services.BankOperations;
using PaymentGatewayService.Services.IService;
using Serilog;

namespace PaymentGatewayService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IWrapperRepository _wrapperRepository;

        private readonly IPaymentRepository _paymentRepository;
        private readonly BankTotalCalculator _bankTotalCalculator;

        public PaymentService(
            IPaymentRepository paymentRepository,
            BankTotalCalculator bankTotalCalculator,
            IWrapperRepository wrapperRepository)
        {
            _wrapperRepository = wrapperRepository;
            _paymentRepository = paymentRepository;
            _bankTotalCalculator = bankTotalCalculator;
        }


        // ----- в данном методе используем dll DatabaseWrapper -----
        public async Task<List<PaymentBanks>> ReadAllAsync()
        {
            try
            {
                var payBankList = await _wrapperRepository.GetAllBank();
                return payBankList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка метод - ReadAllAsync()");
                return null!;
            }
        }

        public async Task<PaymentBanks> ReadAsync(Guid id)
        {
            try
            {
                var payBanks = await _paymentRepository.ReadAsync(id);
                return payBanks;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка при получении объекта с идентификатором {id}", id);
                return null!;
            }
        }

        public async Task<PaymentBanks> CreateAsync(PaymentBanks paymentVm)
        {
            try
            {
                var getBank = await _paymentRepository.ReadAsync(paymentVm.Bank);
                bool isBankEnumValid = Enum.IsDefined(typeof(BankEnum.Bank), paymentVm.Bank);

                // проверяем есть ли банк в BankEnum  
                if (isBankEnumValid)
                {
                    var сalculateTotal = _bankTotalCalculator.CalculateTotal(paymentVm.Bank, paymentVm.Total);
                    //  проверяем есть ли банк в BD 
                    if (getBank == null)
                    {

                        paymentVm.Total += сalculateTotal;
                        paymentVm.Id = Guid.NewGuid();
                        await _paymentRepository.CreateAsync(paymentVm);
                        return paymentVm;
                    }
                    else if(ValidationModels.Valid(paymentVm, "Id"))
                    {
                        // Применяем стратегии
                        getBank.Total += сalculateTotal;
                        await _paymentRepository.UpdateAsync(getBank);
                        return getBank;
                    }

                }
                return getBank;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка при добавление платежа {PaymentBanks}", paymentVm);
                return paymentVm;
            }
        }

        public async Task UpdateAsync(PaymentBanks payment)
        {
            try
            {
                var entity = await _paymentRepository.ReadAsync(payment.Id);
                if (entity != null)
                {
                    entity.Total = payment.Total;
                    await _paymentRepository.UpdateAsync(entity);
                }
                else
                {
                    await CreateAsync(payment);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка при обновлении платежа объект {PaymentBanks}", payment);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {

                var entity = await _paymentRepository.ReadAsync(id);
                if (entity != null)
                {
                    entity.Total = 0;
                    await _paymentRepository.UpdateAsync(entity);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка при обнулении платежа с идентификатором {PaymentId}", id);
            }
        }

    }
}
