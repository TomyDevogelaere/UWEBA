using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseDefaultFiles();
if (!builder.Environment.IsDevelopment())
{
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/html";
        IExceptionHandlerFeature? feature = context.Features.Get<IExceptionHandlerFeature>();
        await context.Response.WriteAsync($"<html><body>Exception: {feature?.Error.Message ?? "Unknown"}</html></body>");
    });
});
}
//app.UseExceptionHandler("/home/error");

//app.Use((context, next) =>
//{
//    Debug.Write(context.Request.Path);
//    return next();
//});

app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
        name:"default",
        pattern:"{controller=Home}/{action=Index}/{id?}"
    );

//app.MapGet("/", () => "Hello World!");

app.Run();
