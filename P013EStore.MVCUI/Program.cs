using P013EStore.Data;
using P013EStore.Service.Abstract;
using P013EStore.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; // Otrum iþlemleri için el ile eklendi..

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();  // Uygulamada session kullanabilmek için 


builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>),typeof(Service<>)); // Kendi yazdýðýmýz Db iþlemlerini yapan servisi .net core da bu þekilde MVC projesine servis olarak tanýtýyoruz ki kullanabilelim.

builder.Services.AddTransient<IProductService, ProductService>(); // Product için yazdýðýmýz özel servisi uygulamaya tanýttýk. AddTransient yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýmda hazýrda ayný nesne varsa o kullanýlýr yoksa yeni bir nesne oluþturulup kullanýma sunulur.

//builder.Services.AddSingleton<IProductService, ProductService>(); // AddSingleton yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýnda bu nesneden 1 tane üretir ve her istekte ayný nesne gönderilir. Performans olarak diðerlerinden iyi yöntemdir.

//builder.Services.AddScoped<IProductService, ProductService>();  // AddScoped yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýnda bu nesneye gelen her istek için ayrý ayrý nesneler üretip bunu kullanýma sunar. Ýçeriðin çok dinamik þekilde sürekli deðiþtiði projelerde kullanýlabilir.( Döviz altýn fiyatý gibi anlýk deðiþimlerin olduðu projelerde...)

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum açmayan kullanýcýlarýn giriþ için gönderileceði adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana eriþim hakký olmayan kullanýcýlarýn gönderileceði sayfa
    x.Cookie.Name = "Administrator"; // Oluþacak cookie'nin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // Oluþacak cookie'nin yaþam süresi ( 1 gün )

}); // Uygulama admin paneli için admin yetkilendirme ayarlarý

builder.Services.AddAuthorization(x=>
{
    x.AddPolicy("AdminPolicy",p=> p.RequireClaim("Role","Admin")); // admin paneline giriþ yapma yetkisine sahip olanlarý bu kuralla kontrol edeceðiz.

    x.AddPolicy("UserPolicy",p=> p.RequireClaim("Role","User")); // admin dýþýnda yetkilendirme kullanýrsak bu kuralý kullanabiliriz(Siteye üye giriþi yapanlarý ön yüzde bir panenle eriþtirme gibi)
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session için 


app.UseAuthentication(); // Oturum açma iþlemine denk gelir.

app.UseAuthorization(); // Oturum yetki sorgulama iþlemine denk gelir.

// Dikkat! Önce UseAuthentication satýrý gelmeli sonra UseAuthorization 


app.MapControllerRoute(
           name: "admin",
           pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
         );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// List<> yerine IEnumerable kullanmayý dene.


// Programý çalýþtýrdýðýmýzda ilk önce add new project diyerek yeni bir libary filtresinden class projesi ekledik