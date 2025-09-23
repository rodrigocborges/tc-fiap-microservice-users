using FIAPCloudGames.API.Endpoints;
using FIAPCloudGames.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UseServices();

app
    .MapUserEndpoints();

app.Run();

