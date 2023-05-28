using Microsoft.EntityFrameworkCore;
using PaymentGatewayService.DataAccess;
using PaymentGatewayService.DataAccess.Repository;
using PaymentGatewayService.DataAccess.Repository.IRepository;
using PaymentGatewayService.Services;
using PaymentGatewayService.Services.BankOperations;
using PaymentGatewayService.Services.IService;
using PaymentGatewayService.Strategies;
using PaymentGatewayService.Strategies.IStrategies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{

    builder.Services.AddControllersWithViews();

    builder.Services.AddControllers();


    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    builder.Services.AddScoped<IWrapperRepository, WrapperRepository>(_ => new WrapperRepository(connectionString!));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

// Repository
    builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Service
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddScoped<BankTotalCalculator>();

// Strategy
    builder.Services.AddScoped<IPaymentStrategy, FirstBankPaymentStrategy>();
    builder.Services.AddScoped<IPaymentStrategy, SecondBankPaymentStrategy>();
    builder.Services.AddScoped<IPaymentStrategy, ThirdBankPaymentStrategy>();
    builder.Services.AddScoped<IPaymentStrategy, DefaultBankPaymentStrategy>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Payment/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Payment}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
