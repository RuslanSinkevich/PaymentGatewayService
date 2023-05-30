$(document).ready(function () {
  const paymentForm = $("#paymentForm");
  initializeFormValidation();


  // Инициализация валидации формы
  function initializeFormValidation() {
    paymentForm.validate(); 
}

  // Валидация формы
  function validation() {
    if (!paymentForm.valid()) {
      // Отображение сообщений об ошибках
      paymentForm.validate().showErrors();
      return false;
    }
    return true;
  }

  // Формируем объект данных для отправки
  function paymentData() {
    var currency = $('input[name="PaymentBanks.Total"]').val();
    var bank = $('select[name="PaymentBanks.Bank"]').val();
    var data = {
      SelectedBank: null,
      PaymentBanks: {
        Total: currency,
        Bank: bank,
      },
    };
    return data;
  }

  $("#submitBtn").click(function (event) {
    event.preventDefault(); // Отменяем стандартное действие кнопки submit
    if (validation()) {   // Валидация формы
      $.ajax({
        url: "/Payment/Create",
        type: "POST",
        data: JSON.stringify(paymentData()),
        contentType: "application/json",
        success: function () {
          // Обработка успешного выполнения запроса
          alert("Данные успешно отправлены.");
        },
        error: function (xhr, textStatus, errorThrown) {
          // Обработка ошибки
          alert("Произошла ошибка при отправке данных: " + errorThrown);
        },
      });
    }
  });
});
