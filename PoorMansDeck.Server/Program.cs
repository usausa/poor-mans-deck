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

// API
builder.ConfigureApi();

// Components
builder.ConfigureComponents();

//--------------------------------------------------------------------------------
// Build host
//--------------------------------------------------------------------------------

var app = builder.Build();

// API
app.MapApi();

// Startup information
app.LogStartupInformation();

// Run
app.RunApplication();
