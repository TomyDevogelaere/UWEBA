using Microsoft.EntityFrameworkCore;
using MinimalAPIContacts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContactDbContext>(options => options.UseInMemoryDatabase("contacts"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/contacts", async (ContactDbContext db) => { 
    return await db.Contacts.ToListAsync();
});

app.MapGet("/api/contacts/{id:int}", async (int id, ContactDbContext db) => {
    Contact? contact = await db.Contacts.SingleOrDefaultAsync(c => c.Id == id);
    if (contact is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(contact);
}).WithName("GetContact");

app.MapPost("/api/contacts", async (Contact input, ContactDbContext db) => {
    Contact? duplicate = await db.Contacts.SingleOrDefaultAsync(c => c.Email == input.Email);
    if (duplicate is not null)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            title: "Duplicates not allowed",
            detail: $"contact with email: {input.Email} already exists"
            );
    }
    db.Contacts.Add(input);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute("GetContact", new {id = input.Id},input);
});

app.Run();

