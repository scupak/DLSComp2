using Common;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
app.UseCors(config => config.AllowAnyOrigin());
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
/*
using var redis = ConnectionMultiplexer.Connect("redis");
IDatabase db = redis.GetDatabase();
db.StringSet("foo", "bar");
Console.WriteLine(db.StringGet("foo"));
Console.WriteLine("Called redis");
*/
/*

var doc = new Document();
doc.Id = 1;
doc.NumberOfAppearances = 1;
doc.Path = "path";
SearchResult testres = new SearchResult{ Documents = new List<Document>{doc}, EllapsedMiliseconds = 1, IgnoredTerms = new List<string>{"test"}};
CachingService.CachingService.SetData("test", testres);
        
SearchResult res = CachingService.CachingService.GetData<SearchResult>("test");
Console.WriteLine("result from redis");
Console.WriteLine(res);
*/