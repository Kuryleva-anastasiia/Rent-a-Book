using Azure;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PoolOfBooks.Data;
using PoolOfBooks.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PoolOfBooksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PoolOfBooksContext") ?? throw new InvalidOperationException("Connection string 'PoolOfBooksContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Подключаю куки
builder.Services.AddAuthentication("Cookies").AddCookie(options => options.LoginPath = "/Users/Login");

builder.Services.AddAuthorization();

// Add ToastNotification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
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

app.UseAuthentication();
app.UseAuthorization();

app.UseNotyf();

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
    var user = context.User;
    if (user.Identity != null)
    {
        if (user.Identity.IsAuthenticated)
        {
            string id = user.FindFirst("ID").Value.ToString();
            if (id != null)
            {
                return Results.Redirect(returnUrl ?? $"~/Users/Details/{id}");
            }
        }
        else { return Results.Redirect(returnUrl ?? "~/Users/LoginNotify"); }
    }
    else { return Results.Redirect(returnUrl ?? "~/Users/LoginNotify"); }
    return Results.Redirect(returnUrl ?? "~/Users/LoginNotify");
});


app.MapGet("/CartAdd/{clientId}/{bookId}/{status}", (string? returnUrl, HttpContext context, PoolOfBooksContext _context, int clientId, int bookId, string status) =>
{

    Cart с = new Cart(clientId, bookId, status);
    _context.Cart.Add(с);
    _context.SaveChanges();
    return Results.Redirect($"~/Carts/CartAddNotify");
});


app.MapGet("/CreateOrderRent", (string? returnUrl, HttpContext context, PoolOfBooksContext _context) =>
{
    var cart = _context.Cart.Include(x => x.Books).Include(x => x.Users).ToList();
    int userId = Convert.ToInt32(context.User.FindFirst("ID").Value);
    var user = _context.Users.FirstOrDefault(x => x.id == userId);

    var books = cart.Where(x => x.userId == userId).Where(x => x.status == "аренда").ToList();
    decimal sum = 0;
    foreach (var book in books)
    {
        sum += Convert.ToDecimal(book.Books.price);
    }
    Order_Rent rent = new Order_Rent(userId, DateTime.Now, DateTime.Now.AddMonths(1), sum, user.address, "Создан");
    _context.Order_Rent.Add(rent);
    _context.SaveChanges();

    var ro = _context.Order_Rent.OrderBy(x => x.id).Last();

    foreach (var book in books)
    {
        Order_Rent_Books orb = new Order_Rent_Books(ro.id, book.bookId);
        _context.Order_Rent_Books.Add(orb);
    }

    _context.SaveChanges();



    _context.Cart.RemoveRange(_context.Cart.Where(x => x.userId == userId).Where(x => x.status == "аренда"));
    _context.SaveChanges();

    return Results.Redirect($"~/Users/Details/{userId}");
});

app.MapGet("/CreateOrderBuy", (string? returnUrl, HttpContext context, PoolOfBooksContext _context) =>
{
    int userId = Convert.ToInt32(context.User.FindFirst("ID").Value);
    var user = _context.Users.FirstOrDefault(x => x.id == userId);

    var cart = _context.Cart.Include(x => x.Books).Include(x => x.Users).ToList();
    var books = cart.Where(x => x.userId == userId).Where(x => x.status == "продажа").ToList();

    decimal sum = 0;

    foreach (var book in books)
    {
        sum += Convert.ToDecimal(book.Books.price);
    }

    try
    {
        Order_Buy buy = new Order_Buy(userId, DateTime.Now, sum, user.address, "Создан");
        _context.Order_Buy.Add(buy);
        _context.SaveChanges();
    } catch (Exception ex) { }

    try
    {
        var ro = _context.Order_Buy.OrderBy(x => x.id).Last();

        foreach (var book in books)
        {
            Order_Buy_Books obb = new Order_Buy_Books(ro.id, book.bookId);
            _context.Order_Buy_Books.Add(obb);
        }

        _context.SaveChanges();
    }catch (Exception ex) { }

    try
    {
        _context.Cart.RemoveRange(_context.Cart.Where(x => x.userId == userId).Where(x => x.status == "продажа"));
        _context.SaveChanges();

    }catch(Exception ex) { }

    return Results.Redirect($"~/Users/Details/{userId}");
});


app.MapGet("/carts/Delete/{cartId}", (string? returnUrl, HttpContext context, PoolOfBooksContext _context, int cartId) =>
{
    int userId = Convert.ToInt32(context.User.FindFirst("ID").Value);
    var c = _context.Cart.FirstOrDefault(x => x.id == cartId);
    _context.Cart.Remove(c);
    _context.SaveChanges();
    return Results.Redirect(returnUrl ?? $"~/Carts/Notify");

});



app.Run();
