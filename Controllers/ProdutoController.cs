using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.ProdutoDto;

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
    }
}
