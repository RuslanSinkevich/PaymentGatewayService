namespace PaymentGatewayService.Strategies.IStrategies
{
    public interface IPaymentStrategy
    {
        decimal CalculatePayment(decimal amount);
    }
}