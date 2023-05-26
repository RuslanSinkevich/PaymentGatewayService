
 ### [Тестовое задание](https://github.com/RuslanSinkevich/PaymentGatewayService/blob/master/test.txt).
 
 В проекте PaymentGatewayService путь DataAccess/Repository/
 находятся два варианта запросов к безе данных
 
 1) PaymentRepository.cs - это стандартный подход через EF
 2) WrapperRepository.cs - это подключённая библиотека DatabaseWrapper.dll где происходит взаимодействие с БД без ОРМ 
   
  в сервисе Service/PaymentService.cs реализован всего один метод обшения с WrapperRepository это ReadAllAsync()   
  остальные работают через PaymentRepository
  
  Также реализованы  тесты - TestPaymentGatewayService.
  
  #### Для тестирования проекта, необходимо изменить в appsettings.json строку подключения к вашей SQL БД создать миграцию и обновить БД
  
  <hr>
  
  #### Визуал
  ![Image alt](https://raw.githubusercontent.com/RuslanSinkevich/PaymentGatewayService/master/img.png)
