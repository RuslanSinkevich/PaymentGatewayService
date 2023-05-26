using PaymentGatewayService.Strategies.IStrategies;

namespace PaymentGatewayService.Strategies
{
    public class SecondBankPaymentStrategy : IPaymentStrategy
    {
        public decimal CalculatePayment(decimal amount)
        {
            return amount - (amount * 0.5m);
        }
    }
}
