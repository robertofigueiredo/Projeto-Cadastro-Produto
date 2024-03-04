using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Projeto_API_Conceitos.Data;
using Projeto_API_Conceitos.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true; // retira os filtros padrões do aspnet
    });

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Configuração do swagger para utilização de terceiros
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o Token JWT dessa maneira: Bearer {seu token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer",
                }
            },

            new string[] { }
        }
    });
});

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>();

// pega o token e gera chave
var JWTSettingsSection = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JWTSettings>(JWTSettingsSection);

var JWTSettings = JWTSettingsSection.Get<JWTSettings>();
var key = Encoding.ASCII.GetBytes(JWTSettings.Segredo);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // padroniza aplicação para usar o jwt
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // precisa trabalhar dentro do https para maior segurança
    options.SaveToken = true; // depois da autorização dando sucesso, voce pode usa-lo novamento
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(key), // formato de chave do JWT
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = JWTSettings.Audiencia,
        ValidIssuer = JWTSettings.Emissor,
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CORS",
                      policy =>
                      {
                          policy.WithOrigins("*")
                                .WithHeaders("*")
                                .WithMethods("*");
                      });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

//app.MapGet("metodo-program", () => "Criado dentro program para teste");

app.Run();
