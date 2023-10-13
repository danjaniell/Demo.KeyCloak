using Application.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(options =>
{
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultAuthenticateScheme = "KeyCloak";
	options.DefaultChallengeScheme = "KeyCloak";
})
.AddCookie()
.AddOpenIdConnect("KeyCloak", options =>
{
	options.ClientId = configuration["KeyCloak:clientId"];
	options.ClientSecret = configuration["KeyCloak:clientSecret"];
	options.MetadataAddress = configuration["KeyCloak:metadataAddress"];
	options.Authority = $"{configuration["KeyCloak:auth-server-url"]}/realms/{configuration["KeyCloak:realm"]}";
	options.ResponseType = OpenIdConnectResponseType.IdTokenToken;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		NameClaimType = "preferred_username",
		RoleClaimType = "roles",
	};
	options.ClaimActions.MapJsonKey("scope", "scope");
	options.Scope.Clear();
	options.Scope.Add("openid");
	options.Scope.Add("profile");
	options.Scope.Add("email");
	options.CallbackPath = new PathString("/callback");
	options.ClaimsIssuer = "KeyCloak";
	options.GetClaimsFromUserInfoEndpoint = true;
	options.SaveTokens = true;
	options.RequireHttpsMetadata = false;
});

var httpClientName = "Default";
builder.Services.AddHttpClient("Default", client => client.BaseAddress = new Uri(configuration["ApiBaseUrl"]));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName));

builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
