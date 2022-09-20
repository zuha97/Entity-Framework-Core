using ContactsAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<Contacts>> GetAllContacts(DataContext context) => 
    await context.contact.ToListAsync();

app.MapGet("/", () => "Welcome to the Contacts DB!");

app.MapGet("/Contact", async (DataContext context) => await context.contact.ToListAsync());

app.MapGet("/Contact/{id}", async (DataContext context, int id) => 
    await context.contact.FindAsync(id) is Contacts Detail ? 
        Results.Ok(Detail) :
        Results.NotFound("Sorry, Detail not found. :/"));

app.MapPost("/Contact", async(DataContext context, Contacts Detail) =>
{
    context.contact.Add(Detail);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllContacts(context));
});

app.MapPut("/Contact/{id}", async (DataContext context, Contacts Detail, int id) =>
{
    var dbcontact = await context.contact.FindAsync(id);
    if (dbcontact == null) return Results.NotFound("No Detail found. :/");

    dbcontact.Name = Detail.Name;
    dbcontact.Email = Detail.Email;
    dbcontact.Address = Detail.Address;
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllContacts(context));

});

app.MapDelete("/Contact/{id}", async (DataContext context, int id) =>
{
    var dbcontact = await context.contact.FindAsync(id);
    if (dbcontact == null) return Results.NotFound("Who's that?");

    context.contact.Remove(dbcontact);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllContacts(context));

});
app.Run();