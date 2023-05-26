using PaymentGatewayService.Strategies;
using PaymentGatewayService.Strategies.IStrategies;

namespace PaymentGatewayService.Services.BankOperations
{
    public class BankTotalCalculator
    {
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

        public BankTotalCalculator(IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _paymentStrategies = paymentStrategies;
        }


        // В данном случае я взял не конкретные банки, а их индексы в перечислении. 
        public decimal CalculateTotal(int bankIndex, decimal amount)
        {
            var selectedStrategy = _paymentStrategies.ElementAtOrDefault(bankIndex);

            if (selectedStrategy != null)
            {
                return selectedStrategy.CalculatePayment(amount);
            }
            
            return GetDefaultStrategy().CalculatePayment(amount);
        }

        private IPaymentStrategy GetDefaultStrategy()
        {
            var selectedStrategy = _paymentStrategies.FirstOrDefault(strategy => strategy is DefaultBankPaymentStrategy);
            return selectedStrategy!;
        }
    }
}