using CubeGame.DAL.Repo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsWebsite.Data.Helper;
using NewsWebsite.Data.Models.Account;
using NewsWebsite.DAL.Data.Context;
using System.Text;
using NewsWebsite.DAL.Repos;
using NewsWebsite.DAL.Data.Models;
using NewsWebsite.BL.Manager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IAuthService, AuthService>();

//-------- he know that we use Identity Role

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("myConn"))
);


/// ---- Repos
builder.Services.AddScoped<IGenericRepository<Author>, GenericRepository<Author>>();
builder.Services.AddScoped<IGenericRepository<News>, GenericRepository<News>>();

builder.Services.AddScoped<IAuthorManager, AuthorManager>();
builder.Services.AddScoped<INewsManager, NewsManager>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7004").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

//builder.Services.AddSession(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;

//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();


app.UseCors("AllowOrigin");


app.UseStaticFiles();

app.MapControllers();
app.Run();
