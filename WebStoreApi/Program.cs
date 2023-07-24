
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebStoreApi.Helpers;
using WebStoreApi.Interfaces;
using WebStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<WebStoreDatabaseSettings>(builder.Configuration.GetSection("WebStoreDatabase"));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IProductsService, ProductsService>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JsonWebToken:Issuer"],
        ValidAudience = builder.Configuration["JsonWebToken:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JsonWebToken:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) 
                .AllowCredentials());

app.MapControllers();

app.Run();
