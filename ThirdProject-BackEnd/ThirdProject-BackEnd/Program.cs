using ChatAppBackEnd.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Services.AuthService;
using ThirdProject_BackEnd.Services.PasswordManager;
using ThirdProject_BackEnd.Services.RefreshTokenService;
using ThirdProject_BackEnd.Services.UserService;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
string AllowCorsOrigin = "_myAllowSpecificOrigins";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgressSQLConnectionString")));
builder.Services.AddScoped<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowCorsOrigin,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
            ClockSkew = TimeSpan.Zero,
        };
    }
);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(AllowCorsOrigin);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
