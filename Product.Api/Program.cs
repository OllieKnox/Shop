using Auth.Extensions;
using Microsoft.EntityFrameworkCore;
using Product.Api.Contexts;
using Product.Api.Converters;
using Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductConverter, ProductConverter>();

builder.Services.AddDbContext<ProductContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();

builder.Services.AddAuthenticationConfig(builder.Configuration);

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("write:products", policy =>
    {
        policy.RequireClaim("permissions", "write:products");
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
