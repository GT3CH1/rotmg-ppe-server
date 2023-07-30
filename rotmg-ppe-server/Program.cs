using Microsoft.EntityFrameworkCore;
using rotmg_ppe_server.data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var deadPlayerConnectionString = builder.Configuration.GetConnectionString("DeadPlayerConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    o => { o.UseLazyLoadingProxies().UseSqlite(connectionString); });
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddLogging();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.InitializeDatabase();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();