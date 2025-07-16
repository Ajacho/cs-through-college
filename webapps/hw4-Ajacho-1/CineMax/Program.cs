using CineMax.Services;
using System.Net.Http.Headers;

namespace CineMax
{ 
    public class Program

    {
    public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string mySecret = builder.Configuration["CineMax2024"];
            string mySecretUrl = "https://api.themoviedb.org/3/";

            builder.Services.AddHttpClient<ICineService, CineService>((httpClient, services) =>
            {
                httpClient.BaseAddress = new Uri(mySecretUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("X-tmdb-Api-Key", mySecret);
                httpClient.DefaultRequestHeaders.Add("X-tmdb-Api-Host", "TMDB-movie-api.com");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mySecret);
                //return new CineService(httpClient, services.GetRequiredService<ILogger<CineService>>());
                return new CineService(httpClient, services.GetRequiredService<ILogger<CineService>>(), builder.Configuration);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
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
        } 
    }
}