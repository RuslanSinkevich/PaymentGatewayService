using PaymentGatewayService.Strategies.IStrategies;

namespace PaymentGatewayService.Strategies
{
    public class FirstBankPaymentStrategy : IPaymentStrategy
    {
        public decimal CalculatePayment(decimal amount)
        {
            return amount * 3;
        }
    }
}
