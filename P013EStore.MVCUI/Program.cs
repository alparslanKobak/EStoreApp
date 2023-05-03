using P013EStore.Data;
using P013EStore.Service.Abstract;
using P013EStore.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>),typeof(Service<>)); // Kendi yazdýðýmýz Db iþlemlerini yapan servisi .net core da bu þekilde MVC projesine servis olarak tanýtýyoruz ki kullanabilelim.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// Programý çalýþtýrdýðýmýzda ilk önce add new project diyerek yeni bir libary filtresinden class projesi ekledik