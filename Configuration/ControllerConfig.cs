namespace Projeto_API_Conceitos.Configuration
{
    public static class ControllerConfig
    {
        public static WebApplicationBuilder AddControllerConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                            .ConfigureApiBehaviorOptions(options =>
                            {
                                options.SuppressModelStateInvalidFilter = true; // retira os filtros padrões do aspnet
                            });
            return builder;
        }
    }
}
