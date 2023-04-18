using GoSales.Utilities.AutoMapper;
using IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// With this line, we inject all dependencies needed to work on the Web Application
builder.Services.InjectDependency(builder.Configuration);

// With this line, we inject the AutoMapper dependency to use it in all the web application
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
