using MuntersInterview.Giphy.Accessor;
using MuntersInterview.Giphy.Cache;
using MuntersInterview.Giphy.Manager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGiphyAccessor(builder.Configuration);
builder.Services.AddGiphyCache();
builder.Services.AddGiphyManager(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
