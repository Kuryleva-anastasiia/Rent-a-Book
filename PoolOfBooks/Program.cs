using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PoolOfBooksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PoolOfBooksContext") ?? throw new InvalidOperationException("Connection string 'PoolOfBooksContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Подключаю куки
builder.Services.AddAuthentication("Cookies").AddCookie(options => options.LoginPath = "/Users/Login");

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/Users/SignIn/{id:int}", async (string? returnUrl, HttpContext context, int id, PoolOfBooksContext _context) =>
{

    var user = _context.Users.FirstOrDefaultAsync(u => u.id ==id);

    if (user.Result != null && user.Result.role != null)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Result.login), new Claim(ClaimTypes.Role, user.Result.role), new Claim("ID", user.Result.id.ToString()) };
        // создаем объект ClaimsIdentity
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        // установка аутентификационных куки
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Results.Redirect($"~/Users/Details/{user.Result.id}");
    }
    return Results.Unauthorized();
});

app.MapGet("/SignInCheckForAvatar", (string? returnUrl, HttpContext context) =>
{
    var user = context.User.Identity;
    if (user != null)
    {
        if (user.IsAuthenticated)
        {
            string id = context.User.FindFirst("ID").Value.ToString();
            if (id != null)
            {
                return Results.Redirect(returnUrl ?? $"~/Users/Details/{id}");
            }
        }
        else { return Results.Redirect(returnUrl ?? "~/Users/Login"); }
    }
    else { return Results.Redirect(returnUrl ?? "~/Users/Login"); }
    return Results.Redirect(returnUrl ?? "~/Users/Login");
});



app.MapGet("/CartAdd/{clientId}/{bookId}/{status}", (string? returnUrl, HttpContext context, PoolOfBooksContext _context, int clientId, int bookId, string status) =>
{
    var cart = new Cart(clientId, bookId, status);
    _context.Cart.Add(cart);
    _context.SaveChanges();
    return Results.Redirect($"~/Carts/Details/{clientId}");
});

app.Run();
