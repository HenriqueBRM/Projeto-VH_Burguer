namespace VH_Burguer.DTOs.ProdutoDto
{
    public class AtualizarProdutoDto
    {
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = null!;
        public IFormFile Imagem { get; set; }

        public List<int> CategoriaIds { get; set; }
        public bool ? StatusProduto { get; set; }

    }
}
