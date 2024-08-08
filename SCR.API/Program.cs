using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this namespace
using SCR.API.Data;
using System.Text;
using SCR.API.Config;
using Microsoft.AspNetCore.Identity;
using SCR.API.Repositories;
using Microsoft.OpenApi.Models;
using System.Security.AccessControl;
using System.Web.Http.Cors;
using NsfwSpyNS;
using SCR.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors( options =>
{
    options.AddPolicy(name: "reactProject", policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
} );
builder.Services.AddSwaggerGen(Options =>
{
Options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SCR api", Version = "v1" });
Options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = JwtBearerDefaults.AuthenticationScheme
});
    Options.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type= ReferenceType.SecurityScheme,
                Id= JwtBearerDefaults.AuthenticationScheme
            },
            Scheme="Oauth2",
            Name=JwtBearerDefaults.AuthenticationScheme,
            In=ParameterLocation.Header
        },
        new List<string>()
    }
}); ;
});
builder.Services.AddDbContext<SCRDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SCRConnectionString")));
builder.Services.AddDbContext<SCRAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SCRAuthConnectionString")));
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("SCR")
    .AddEntityFrameworkStores<SCRAuthDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(Options =>
{
    Options.Password.RequireDigit = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequiredLength = 3;
    Options.Password.RequiredUniqueChars = 1;

});

// Add your JWT authentication service here
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});
// registerationservice
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<INsfwSpy, NsfwSpy>();
builder.Services.AddScoped<NotificationService>();
//app bulding
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("reactProject");
app.Run();
