using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayService.Helpers
{
    public static class ValidationModels
    {
        /// <summary>
        /// Проверяем валидность данных с моделью
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="remove"></param>
        /// <returns>bool</returns>
        public static bool Valid<T>(T value, string remove)
        {
            var validationContext = new ValidationContext(value!);
            var validationResults = new List<ValidationResult>();

            validationContext.MemberName = remove;

            bool isValid = Validator.TryValidateObject(value!, validationContext, validationResults, true);

            return isValid;
        }
    }
}
