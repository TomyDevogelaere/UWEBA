WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<PeopleContext>(
//  options => options.UseInMemoryDatabase("SuperSecretDatabase"));

builder.Services.AddDbContext<PeopleContext>(
    options => options.UseInMemoryDatabase(
        builder.Configuration.GetConnectionString("PeopleDatabase")));

var features = new Features();
builder.Configuration.Bind("Features", features);
builder.Services.AddSingleton<Features>(features);

WebApplication? app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
  PeopleContext db = scope.ServiceProvider.GetRequiredService<PeopleContext>();
  db.Database.EnsureCreated();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=People}/{action=Index}/{id?}");

app.Run();
