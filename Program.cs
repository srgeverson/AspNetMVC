using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.Cookie.HttpOnly = true;
      options.Cookie.SecurePolicy = true
        ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
      options.Cookie.SameSite = SameSiteMode.Lax;
      options.Cookie.Name = "SimpleTalk.AuthCookieAspNetCore";
      options.LoginPath = "/Login";
      options.LogoutPath = "/Home";
  });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.None;
    options.Secure = true
      ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
});

builder.Services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

#region Rotas

//app.MapControllerRoute(
//    name: "sair",
//    pattern: "{controller=Login}/{action=Sair}");

app.MapControllerRoute(
name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}");
#endregion

app.Run();
