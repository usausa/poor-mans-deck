using Microsoft.AspNetCore.Builder;

using PoorMansDeck.Server;

//--------------------------------------------------------------------------------
// Configure builder
//--------------------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// System
builder.ConfigureSystemDefaults();

// Logging
builder.ConfigureLoggingDefaults();

// gRPC
builder.ConfigureGrpcService();

// Components
builder.ConfigureComponents();

//--------------------------------------------------------------------------------
// Build host
//--------------------------------------------------------------------------------

var app = builder.Build();

// gRPC
app.MapGrpcService();

// Startup information
app.LogStartupInformation();

// Run
app.RunApplication();
