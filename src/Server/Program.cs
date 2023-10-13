using Keycloak.AuthServices.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if (builder.Environment.IsDevelopment())
{
	services.AddKeycloakAuthentication(configuration, o =>
	{
		o.RequireHttpsMetadata = false;
	});
}
else
{
	services.AddKeycloakAuthentication(configuration);
}
services.AddControllers();
services.AddEndpointsApiExplorer();

var openIdConnectUrl = $"{configuration["Keycloak:auth-server-url"]}/"
 + $"realms/{configuration["Keycloak:realm"]}/"
 + ".well-known/openid-configuration";

services.AddSwaggerGen(c =>
{
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "Auth",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.OpenIdConnect,
		OpenIdConnectUrl = new Uri(openIdConnectUrl),
		Scheme = "bearer",
		BearerFormat = "JWT",
		Reference = new OpenApiReference
		{
			Id = "Bearer",
			Type = ReferenceType.SecurityScheme
		}
	};
	c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{securityScheme, Array.Empty<string>()}
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
