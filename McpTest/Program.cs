using McpTest.Interfaces;
using McpTest.Middlewares;
using McpTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

builder.Services.AddScoped<IProductService,ProductService>();

builder.Services.AddHttpClient("myproductapi", options =>
{
    options.BaseAddress = new Uri("https://localhost:7183/api/");
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGroup("/mcp").MapMcp();

app.MapGet("/", () => "Hello World!");

app.Run();
