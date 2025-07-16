using CineProject.Models;
using Microsoft.EntityFrameworkCore;
using CineProject.DAL.Abstract;
using CineProject.DAL.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<StreamingDBDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StreamingDBDbContext")));

builder.Services.AddScoped<DbContext, StreamingDBDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registering IShowRepository with its implementation
builder.Services.AddScoped<IShowRepository, ShowRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}else
    {
        app.UseDeveloperExceptionPage();
    }

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();