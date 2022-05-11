using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ServerSide.Data;
using Microsoft.Extensions.Configuration;
using System;
using ServerSide.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServerSideContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerSideContext") ?? throw new InvalidOperationException("Connection string 'ServerSideContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<RatingService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {

    options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTParams:Audience"],
            ValidIssuer = builder.Configuration["JWTParams:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTParams:SecretKey"]))
        };
}
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All", builder =>
     {
         builder
           .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();

     });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Allow All");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();
