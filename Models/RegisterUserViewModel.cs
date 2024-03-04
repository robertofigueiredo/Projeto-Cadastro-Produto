using System.ComponentModel.DataAnnotations;

namespace Projeto_API_Conceitos.Models
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está com o formato inválido")]
        public string Email { get;set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres")]
        public string Password { get;set; }

        [Compare("Password",ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get;set; }
    }
}
