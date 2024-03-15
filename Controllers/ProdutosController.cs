using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_API_Conceitos.Data;
using Projeto_API_Conceitos.Models;


namespace Projeto_API_Conceitos.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/produto")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ProdutosController(ApiDbContext context)
        {
            _context = context;
        }

        //[AllowAnonymous] // retira necessidade de autorização
        [HttpGet("GetAllProduto")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
        {
            var retorno = await _context.produtos.ToListAsync();
            return Ok(retorno);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutoId(int id)
        {
            var retorno = await _context.produtos.FindAsync(id);
            return Ok(retorno);
        }

        [HttpPost("CadastroProduto")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Produto>> Post([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Detail = "Os Parametros fornecidos são invalidos"
                });
            }
            _context.produtos.Add(produto);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Produto cadastrado com sucesso");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Produto>> Put(int id, Produto produto)
        {
            if (produto.Id != id)
            {
                return BadRequest("Id invalido");
            }

            _context.produtos.Update(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            if (_context.produtos == null)
            {
                return NotFound();
            }

            var retorno = await _context.produtos.FindAsync(id);

            if (retorno == null)
            {
                return NotFound();
            }

            _context.produtos.Remove(retorno);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
