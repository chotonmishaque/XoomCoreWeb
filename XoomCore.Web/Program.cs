using XoomCore.Infrastructure;
using XoomCore.Infrastructure.Logging.Serilog;
using XoomCore.Services;
using XoomCore.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.AddConfigurations().RegisterSerilog();
builder.Services.AddInfrastructures();
builder.Services.AddServices();
// Other service registrations

builder.Services.AddSession();

var app = builder.Build();
app.UseServices();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();