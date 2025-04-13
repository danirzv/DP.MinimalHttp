using MinimalHttp.Sample;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<TargetHttpClientOptions>(configuration.GetSection("TargetHttpClientOptions"));
services.AddHttpClient<TargetClient, TargetHttpClientOptions>();

var app = builder.Build();

app.MapGet("/successful", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/83dc5d78-9e6e-4edd-916a-69d8fc227813"));
app.MapGet("/interesting", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/7835422b-622d-4ff5-a0d6-4115d7156baf"));
app.MapGet("/uninteresting", (TargetClient targetClient) => targetClient.CoolMethodCall("/v3/c191b468-defe-459a-a6a3-237a6464c58d"));

app.Run();

