using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum açmayan kullanýcýlarýn giriþ için gönderileceði adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana eriþim hakký olmayan kullanýcýlarýn gönderileceði sayfa
    x.Cookie.Name = "Administrator"; // Oluþacak cookie'nin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // Oluþacak cookie'nin yaþam süresi ( 1 gün )

}); // Uygulama admin paneli için admin yetkilendirme ayarlarý

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role", "Admin")); // admin paneline giriþ yapma yetkisine sahip olanlarý bu kuralla kontrol edeceðiz.

    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role", "User")); // admin dýþýnda yetkilendirme kullanýrsak bu kuralý kullanabiliriz(Siteye üye giriþi yapanlarý ön yüzde bir panenle eriþtirme gibi)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
           name: "admin",
           pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
         );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
