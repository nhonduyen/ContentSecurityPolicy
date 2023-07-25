using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(config =>
{
    config.InputFormatters.OfType<SystemTextJsonInputFormatter>().First().SupportedMediaTypes.Add(
            new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/csp-report")
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    // enable for wss
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' wss://localhost:44379; report-uri home/cspreport"); // prevent xss
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // cannot add frame our site
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // prevent hacker sniffing missing metadata on served file
    context.Response.Headers.Add("Referrer-Policy", "no-referrer"); // prevent dispose where the client come from which website
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()"); // permission on device
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
