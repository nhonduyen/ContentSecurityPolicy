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
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src 'self' wss:; report-uri /csp/cspreport;"); // prevent xss
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // cannot add frame our site
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // prevent hacker sniffing missing metadata on served file
    context.Response.Headers.Add("Referrer-Policy", "same-origin"); // prevent dispose where the client come from which website; no-referrer
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()"); // permission on device
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains;");
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
