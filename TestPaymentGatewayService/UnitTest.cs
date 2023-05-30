using Moq;
using PaymentGatewayService.Strategies.IStrategies;
using PaymentGatewayService.Strategies;
using PaymentGatewayService.Services.BankOperations;
using PaymentGatewayService.Services;
using PaymentGatewayService.DataAccess.Repository.IRepository;
using PaymentGatewayService.Models;
using System.ComponentModel.DataAnnotations;

namespace TestPaymentGatewayService
{
    public class UnitTest
    {
        private readonly IPaymentStrategy _firstBankStrategy = new FirstBankPaymentStrategy();
        private readonly IPaymentStrategy _secondBankStrategy = new SecondBankPaymentStrategy();
        private readonly IPaymentStrategy _thirdBankStrategy = new ThirdBankPaymentStrategy();
        private readonly IPaymentStrategy _defaultStrategy = new DefaultBankPaymentStrategy();

        private readonly BankTotalCalculator _bankTotalCalculator;

        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IWrapperRepository> _wrapperRepository;
        private readonly PaymentService _paymentService;


        public UnitTest()
        {
            // заглушка для IPaymentRepository
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _wrapperRepository = new Mock<IWrapperRepository>();

            var paymentStrategies = new List<IPaymentStrategy>
            {
                _firstBankStrategy,
                _secondBankStrategy,
                _thirdBankStrategy,
                _defaultStrategy
            };

            _bankTotalCalculator = new BankTotalCalculator(paymentStrategies);

            // экземпляр PaymentService  заглушки
            _paymentService = new PaymentService(
                _paymentRepositoryMock.Object,  
                _bankTotalCalculator, 
                _wrapperRepository.Object);
        }

        [Fact]
        public Task GetAllBanksAsync_Valid_Models()
        {
            // Arrange
            var paymentVm = new PaymentBanks
            {
                Bank = 3,
                Total = 1
            };

            _wrapperRepository.Setup(r => r.GetAllBank())
                .ReturnsAsync(new List<PaymentBanks>());
            var context = new ValidationContext(paymentVm);
            var result = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paymentVm, context, result);

            // Assert

            Assert.True(isValid);
            Assert.NotNull(result);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task CreateAsync_ValidPayment_ReturnsCreatedPayment()
        {
            // Arrange
            var paymentVm = new PaymentBanks
            {
                Bank = 3,
                Total = 1
            };

            _paymentRepositoryMock.Setup(r => r.ReadAsync(paymentVm.Bank))
                .ReturnsAsync((PaymentBanks)null!);

            var context = new ValidationContext(paymentVm);
            var results = new List<ValidationResult>();

            // Act
            var result = await _paymentService.CreateAsync(paymentVm);
            var isValid = Validator.TryValidateObject(paymentVm, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.NotNull(result);
            Assert.Equal(paymentVm, result);
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(160, 480)]
        [InlineData(300, 900)]
        [InlineData(0, 0)]
        public void Bank_Total_Calculator(int payments, int result)
        {
            var calculator = _bankTotalCalculator.CalculateTotal(0, payments);
            Assert.Equal( result, calculator);
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(160, 480)]
        [InlineData(300, 900)]
        [InlineData(10000, 30000)]
        [InlineData(0, 0)]
        public void First_Bank_Strategy(int payments, int result)
        {
            var firstBankResult = _firstBankStrategy.CalculatePayment(payments);
            Assert.Equal(result, firstBankResult);
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(160, 80)]
        [InlineData(300, 150)]
        [InlineData(10000, 5000)]
        [InlineData(0, 0)]
        public void Second_Bank_Strategy(int payments, int result)
        {
            decimal secondBankResult = _secondBankStrategy.CalculatePayment(payments);
            Assert.Equal(result, secondBankResult);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(160, 140)]
        [InlineData(300, 350)]
        [InlineData(10000, 14900)]
        [InlineData(0, 0)]
        public void Third_Bank_Strategy(int payments, int result)
        {
            decimal thirdBankResult = _thirdBankStrategy.CalculatePayment(payments);
            Assert.Equal(result, thirdBankResult);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(160, 160)]
        [InlineData(300, 300)]
        [InlineData(10000, 10000)]
        [InlineData(0, 0)]
        public void Default_Strategy(int payments, int result)
        {
            decimal defaultStrategy = _defaultStrategy.CalculatePayment(payments);
            Assert.Equal(result, defaultStrategy);

        }

 

    }
}