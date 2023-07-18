using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using WebStoreApi.Authorization;
using WebStoreApi.Collections;
using WebStoreApi.Helpers;
using WebStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<WebStoreDatabaseSettings>(builder.Configuration.GetSection("WebStoreDatabase"));
builder.Services.Configure<WebStoreAppSettings>(builder.Configuration.GetSection("JsonWebToken"));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUsersService, UsersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
