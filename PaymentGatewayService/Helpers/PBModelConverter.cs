

namespace PaymentGatewayService.Helpers
{
    public static class PBModelConverter
    {
        /// <summary>
        /// Конвертирует из PaymentGatewayService в DatabaseWrapper
        /// </summary>
        /// <param name="source"></param>
        /// <returns>DatabaseWrapper.Models</returns>
        public static DatabaseWrapper.Models.PaymentBanks ConvertToPGS(Models.PaymentBanks source)
        {
            return new DatabaseWrapper.Models.PaymentBanks
            {
                Id = source.Id,
                Bank = source.Bank,
                Total = source.Total
            };
        }

        /// <summary>
        /// Конвертирует из DatabaseWrapper в PaymentGatewayService 
        /// </summary>
        /// <param name="source"></param>
        /// <returns>DatabaseWrapper.Models</returns>
        public static Models.PaymentBanks ConvertToDW(DatabaseWrapper.Models.PaymentBanks source)
        {
            return new Models.PaymentBanks
            {
                Id = source.Id,
                Bank = source.Bank,
                Total = source.Total
            };
        }

        /// <summary>
        /// Конвертирует из DatabaseWrapper в PaymentGatewayService (List)
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns>List->PaymentGatewayService.Models</returns>
        public static List<Models.PaymentBanks> ConvertPGSToList(List<DatabaseWrapper.Models.PaymentBanks> sourceList)
        {
            return sourceList.Select(pb => new Models.PaymentBanks
            {
                Id = pb.Id,
                Bank = pb.Bank,
                Total = pb.Total
            }).ToList();
        }
    }
}
