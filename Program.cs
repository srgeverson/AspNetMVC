using AspNetMVC.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Name = String.Format("AuthCookieAspNetCore.{0}", AppDomain.CurrentDomain.FriendlyName.Split('.')[0]);
        options.LoginPath = "/Login";
        options.LogoutPath = "/Home";
    });

builder.Services
    .Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.Strict;
        options.HttpOnly = HttpOnlyPolicy.None;
        options.Secure = CookieSecurePolicy.Always;
    });

//IoC
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ISessaoUsuarioHelper, SessaoUsuarioHelper>();

//Session
builder.Services.AddSession(s =>
{
    s.Cookie.HttpOnly = true;
    s.Cookie.IsEssential = true;
});

builder.Services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));

builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

app.UseCookiePolicy();
app.UseAuthentication();

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

app.UseAuthorization();

app.UseSession();

#region Rotas

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}");

//app.MapControllerRoute(name: "erro", pattern: "{controller=Home}/{action=Error}");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();
