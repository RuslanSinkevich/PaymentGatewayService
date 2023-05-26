using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaymentGatewayService.Models.ViewModels
{
    public class PaymentViewModel
    {
        public IEnumerable<SelectListItem>? SelectedBank { get; set; }
        public PaymentBanks? PaymentBanks { get; set; }
    }
}
