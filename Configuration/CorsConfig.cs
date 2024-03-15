namespace Projeto_API_Conceitos.Configuration
{
    public static class CorsConfig
    {
        public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                // exemplo do CORS Geral
                options.AddPolicy("Development", builder =>
                                      builder
                                      .AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());

                // exemplo do CORS específico
                options.AddPolicy("Production", builder =>
                                    builder
                                    .WithOrigins("sua url específica")
                                    .WithMethods("POST")
                                    .AllowAnyHeader());
            });
            return builder;
        }
    }
}
