using System;

namespace DatabaseWrapper.Models
{
    public class PaymentBanks
    {

        public Guid Id { get; set; }

        public int Bank { get; set; }

        public decimal Total { get; set; }
    }
}
