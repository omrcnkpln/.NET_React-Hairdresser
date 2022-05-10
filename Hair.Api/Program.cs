using Hair.Api;
using Hair.Repository;
using Hair.Service;
using Hair.Service.AutoMapper.UserProfile;
using Hair.Service.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration Configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

//middleware tanımla
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, file);
    c.IncludeXmlComments(filePath);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        var JwtKey = Encoding.ASCII.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(Configuration["TokenSecret"])));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(JwtKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateLifetime=true // E�er s�resi ge�mi� bir token gelirse invalid yap�lmas�n� sa�l�yor.
        };
    });

// Custome Authorization Handler
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActionUserPolicy", policy =>
    {
        policy.AddRequirements(new AuthorizationRequirement());
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAutoMapper(
    typeof(UserProfile)
);

builder.Services.AddRepositories(Configuration);
builder.Services.AddCustomeServices();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Hair.Session";
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    DbSeeder.Seed(app);

    //Global Error Handling
    //app.UseExceptionHandler("/Error");
    ExceptionHandler.ConfigureExceptionHandler(app);
}
else
{
    //app.UseExceptionHandler("/Error");
    ExceptionHandler.ConfigureExceptionHandler(app);
}

app.UseSession();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
