using DapperMegaSalesAnalytics.BusinessLayer.Abstract;
using DapperMegaSalesAnalytics.BusinessLayer.Concrete;
using DapperMegaSalesAnalytics.DataAccessLayer.Abstract;
using DapperMegaSalesAnalytics.DataAccessLayer.Concrete;
using DapperMegaSalesAnalytics.DataAccessLayer.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<ISalesTransactionDal, SalesTransactionDal>();
builder.Services.AddScoped<ISalesTransactionService, SalesTransactionManager>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();