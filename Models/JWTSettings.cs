namespace Projeto_API_Conceitos.Models
{
    public class JWTSettings
    {
        public string? Segredo { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string? Emissor { get; set; }
        public string? Audiencia { get; set; }
    }
}
