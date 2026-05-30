using MuntersInterview.Giphy.Accessor;
using MuntersInterview.Giphy.API;
using MuntersInterview.Giphy.Cache;
using MuntersInterview.Giphy.Manager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddGiphyAccessor(builder.Configuration);
builder.Services.AddGiphyCache();
builder.Services.AddGiphyManager(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
