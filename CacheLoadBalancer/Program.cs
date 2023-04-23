using CacheLoadBalancer.Infrastructure;
using CacheLoadBalancer.LoadBalancer;
using CacheLoadBalancer.Strategies;
using Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IServiceGateway<SearchResult>, CacheServiceGateway>();

builder.Services.AddSingleton<ILoadBalancerStrategy, RoundRobinStrategy>();
builder.Services.AddSingleton<ILoadBalancer, LoadBalancer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
