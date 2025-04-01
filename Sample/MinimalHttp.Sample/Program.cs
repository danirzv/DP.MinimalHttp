using MinimalHttp.Sample;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<TargetHttpClientOptions>(configuration.GetSection("TargetHttpClientOptions"));
services.AddHttpClient<TargetClient, TargetHttpClientOptions>();

var app = builder.Build();

app.MapGet("/successful", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/309893fe-0097-43da-9a83-448ff2eafb2e"));
app.MapGet("/interesting", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/0ff48ca6-a1bf-4e7d-90b2-52201ecfc6f2"));
app.MapGet("/uninteresting", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/ced34a23-bfe4-4e82-9f6a-3401377d6418"));

app.Run();

