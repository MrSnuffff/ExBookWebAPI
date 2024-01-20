using ExBookWebAPI.OtherAPIs;
using Microsoft.EntityFrameworkCore;
using ExBookWebAPI.Data;
using ExBookWebAPI.Security;
using ExBookWebAPI.Middlware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<SearchBookInOtherAPIs>();
builder.Services.AddScoped<SearchBookInOtherAPIs>();


var conectionstring = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(conectionstring, ServerVersion.AutoDetect(conectionstring)));

var app = builder.Build();


app.UseHttpsRedirection();

app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.Run();
