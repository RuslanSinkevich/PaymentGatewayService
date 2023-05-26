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

        [Fact]
        public void Bank_Total_Calculator()
        {
            var calculator = _bankTotalCalculator.CalculateTotal(0, 100);
            Assert.Equal(300, calculator);
        }

        [Fact]
        public void First_Bank_Strategy()
        {
            decimal firstBankResult = _firstBankStrategy.CalculatePayment(100);
            Assert.Equal(300, firstBankResult);
        }

        [Fact]
        public void Second_Bank_Strategy()
        {
            decimal secondBankResult = _secondBankStrategy.CalculatePayment(100);
            Assert.Equal(50, secondBankResult);
        }

        [Fact]
        public void Third_Bank_Strategy()
        {
            decimal thirdBankResult = _thirdBankStrategy.CalculatePayment(100);
            Assert.Equal(50, thirdBankResult);
        }

        [Fact]
        public void Default_Strategy()
        {
            decimal defaultStrategy = _defaultStrategy.CalculatePayment(100);
            Assert.Equal(100, defaultStrategy);

        }

 

    }
}