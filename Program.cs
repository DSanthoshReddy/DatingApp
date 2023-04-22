using API.Services;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddDbContext<API.DbDataContext.DataContext>(opt=>
    {
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build();

app.UseCors(bd => bd.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
