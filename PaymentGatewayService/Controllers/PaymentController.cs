using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentGatewayService.Enums;
using PaymentGatewayService.Models;
using PaymentGatewayService.Models.ViewModels;
using PaymentGatewayService.Services.IService;
using System.Diagnostics;

namespace PaymentGatewayService.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var paymentVm = new PaymentViewModel
            {
                SelectedBank = Enum.GetValues<BankEnum.Bank>().Select(u => new SelectListItem
                {
                    Value = ((int)u).ToString(),
                    Text = u.ToString()
                }).ToList(),
            };
            return View(paymentVm);
        }

        [HttpGet]
        [Route("/GetAll")]
        public async Task<List<PaymentBanks>> GetAll()
        {
            return await _paymentService.ReadAllAsync();
        }

        [HttpGet]
        [Route("/Delete/{id:Guid}")]
        public async Task Delete(Guid id)
        {
            await _paymentService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task Update([FromBody] PaymentViewModel paymentVm)
        {
            await _paymentService.UpdateAsync(paymentVm.PaymentBanks!);
        }

        [HttpPost]
        public async Task Create([FromBody] PaymentViewModel paymentVm)
        {
            await _paymentService.CreateAsync(paymentVm.PaymentBanks!);
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
