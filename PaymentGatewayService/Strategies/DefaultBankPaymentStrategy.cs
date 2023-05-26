using PaymentGatewayService.Strategies.IStrategies;

namespace PaymentGatewayService.Strategies
{
    public class DefaultBankPaymentStrategy : IPaymentStrategy
    {
        public decimal CalculatePayment(decimal amount)
        {
            return amount;
        }
    }
}
