using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoBackend.Data;
using TodoBackend.Interfaces;
using TodoBackend.Services;
using TodoBackend.tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��ӷ�������
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ע�� IMemoryCache ����
builder.Services.AddMemoryCache();

builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtToken"));

// JWT ����
var jwtSettings = builder.Configuration.GetSection("JwtToken").Get<JwtTokenSettings>();

builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtToken"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtTokenSettings>>().Value);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.SiteUrl,
            ValidAudience = jwtSettings.SiteUrl,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddCors(options => {
    options.AddPolicy("VueCors", policy => {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<JwtHelper>();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("VueCors");

app.UseHttpsRedirection();

// ȷ���� UseAuthorization ֮ǰ���� UseAuthentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
