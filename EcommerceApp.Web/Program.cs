using EcommerceApp.Application;
using EcommerceApp.Infrastructure;
using EcommerceApp.Infrastructure.Seeds;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject Assembly References Services
builder.Services
    .AddInfrastructure(configuration)
    .AddApplication();

var app = builder.Build();

// Seed Roles and Superadmin
using(var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    await DataSeeder.Seed(service);

}

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

app.Run();
