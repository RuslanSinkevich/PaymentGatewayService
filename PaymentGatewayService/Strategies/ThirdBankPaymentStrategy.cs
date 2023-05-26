using PaymentGatewayService.Strategies.IStrategies;

namespace PaymentGatewayService.Strategies
{
    public class ThirdBankPaymentStrategy : IPaymentStrategy
    {
        public decimal CalculatePayment(decimal amount)
        {
            decimal result = amount + (amount * 0.5m) - 100;
            return Math.Max(result, 0);
        }
    }
}
