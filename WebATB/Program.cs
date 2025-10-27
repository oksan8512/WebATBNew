using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebATB.Data;
using WebATB.Data.Entities.Idenity;
using WebATB.Extensions;
using WebATB.Interfaces;
using WebATB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppATBDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionATB")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppATBDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// ⬇️ СПОЧАТКУ створюємо папки для зображень
Dictionary<string, string> imageSizes = new()
{
    { "ImagesDir", "images" },
    { "AvatarDir", "avatars" },
};

foreach (var (key, value) in imageSizes)
{
    var dirName = builder.Configuration.GetValue<string>(key) ?? value;
    var dir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
    Directory.CreateDirectory(dir);
}

// ⬇️ ПОТІМ налаштовуємо статичні файли
app.UseStaticFiles(); // wwwroot

// Додаткові статичні файли для images та avatars
foreach (var (key, value) in imageSizes)
{
    var dirName = builder.Configuration.GetValue<string>(key) ?? value;
    var dir = Path.Combine(Directory.GetCurrentDirectory(), dirName);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(dir),
        RequestPath = $"/{value}"
    });
}

app.UseRouting();

app.UseAuthentication(); // ⬅️ ОБОВ'ЯЗКОВО!
app.UseAuthorization();

// Маршрути
app.MapAreaControllerRoute(
    name: "MyAreaAdmin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");

await app.SeedDataAsync();

app.Run();
