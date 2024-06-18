using EF.DBFirst.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Apbd8Context, Apbd8Context>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
        
app.Run();
