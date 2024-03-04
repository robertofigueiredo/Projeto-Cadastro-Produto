using System.ComponentModel.DataAnnotations;

namespace Projeto_API_Conceitos.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "É necessário informar o preço do produto")]
        [Range(1,int.MaxValue,ErrorMessage = "O preço deve ser maior que 0")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "A QuantidadeEstoque é obrigatória")]
        public int QuantidadeEstoque { get; set; }

        [StringLength(25)]
        public string? Descricao { get; set; }
    }
}
