using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Projeto_API_Conceitos.Data;
using Projeto_API_Conceitos.Models;
using System.Runtime.CompilerServices;
using System.Text;
namespace Projeto_API_Conceitos.Configuration
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
        {
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
                options.SaveToken = true; // depois da autorização dando sucesso, voce pode usa-lo novamente
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key), // formato de chave do JWT
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = JWTSettings.Audiencia,
                    ValidIssuer = JWTSettings.Emissor,
                };
            });

            return builder;
        }
    }
}
