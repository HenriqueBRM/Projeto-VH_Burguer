using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.ProdutoDto;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        // Autenticacao do usuario

        private int ObterUsuarioIdLogado()
        {
            // busca no token/claims o valor armazenado como id do usuario
            // ClaimTypes.NameIdentifier geralmente guarda o ID do usuario no JWT
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(idTexto))
            {
                throw new DomainException("Usuario nao autenticado");
            }

            // As Claims(informacoes do usuario dentro do token) sempre serao armazenadas como texto
            return int.Parse(idTexto);
        }


        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            List<LerProdutoDto> produtos = _service.Listar();
            return StatusCode(200, produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto>ObterPorId(int id)
        {
            LerProdutoDto produto = _service.ObterPorId(id);
            if(produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpGet("{id}/imagem")]

        public ActionResult ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);

                // Retorna o arquivo pro navegador "image/jpeg" informa o tipo da imagem (MIME type)
                // O navegador entende que deve renderizar como imagem
                return File(imagem, "imagem/jpeg");
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
