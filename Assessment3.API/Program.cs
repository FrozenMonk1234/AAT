using Assessment3.API.Repository;
using Assessment3.API.Repository.Implementation;
using Assessment3.API.Services;
using Assessment3.API.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Dependencies
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ISQLiteService, SQLiteService>();
builder.Services.AddCors(options =>
{

    options.AddPolicy("Debug", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader()
        .WithOrigins(builder.Configuration["ConnectionStrings:ClientUri"]!);
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Debug");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
