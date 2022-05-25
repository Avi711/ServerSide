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
using ServerSide.Hubs;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServerSideContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerSideContext") ?? throw new InvalidOperationException("Connection string 'ServerSideContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

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

builder.Services.AddSingleton<IDictionary<string, string>>(opt => new Dictionary<string, string>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All", builder =>
     {
         //builder
         //  .AllowAnyOrigin()
         //   .AllowAnyMethod()
         //   .AllowAnyHeader();
         builder.WithOrigins("http://localhost:3000")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();

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
    pattern: "{controller=Ratings}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<Myhub>("/myHub");
});

app.Run();
