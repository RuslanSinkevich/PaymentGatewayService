using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PaymentGatewayService.Enums;

namespace PaymentGatewayService.Models
{
    [Table("banks_total")]
    public class PaymentBanks
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение {0} должно быть от {1} до {2}.")]
        [EnumDataType(typeof(BankEnum.Bank))]
        [Column("bank")]
        public int Bank { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Цена должна быть больше нуля!")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Поле {0} должно быть положительным числом.")]
        [Column("total")]
        public decimal Total { get; set; }
    }
}
     