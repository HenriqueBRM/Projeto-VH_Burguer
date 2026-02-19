using VH_Burguer.Applications.Conversoes;
using VH_Burguer.Applications.Regras;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.ProdutoDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;


namespace VH_Burguer.Applications.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        //Para cada produto que veio do banco cria um DTO so com o que a requisicao/front precisa
        public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.Listar();
            List<LerProdutoDto> ProdutosDto = produtos.Select(ProdutoParaDto.ConverterParaDto).ToList();

            return ProdutosDto;
        }

        public LerProdutoDto ObterPorId(int id)
        {
            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new DomainException("Produto nao encontrado");
            }

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        private static void ValidarCadastro(CriarProdutoDto produtoDto)
        {
            if (string.IsNullOrWhiteSpace(produtoDto.Nome))
            {
                throw new DomainException("Nome eh obrigatorio");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preco deve ser maior do que zero");
            }

            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
            {
                throw new DomainException("Descricao eh obrigatoria");
            }

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
            {
                throw new DomainException("Imagem eh obrigatoria");
            }

            if (produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria");
            }
        }

        public byte[] ObterImagem(int id)
        {
            var imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem nao encontrada");
            }

            return imagem;
        }

        public LerProdutoDto Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto);

            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Um produto com esse nome ja existe");
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = produtoDto.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioID = usuarioId
            };

            _repository.Adicionar(produto, produtoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        public LerProdutoDto Atualizar(int id, AtualizarProdutoDto produtoDto)
        {
            HorarioAlteracaoProduto.ValidarHorario();
            Produto produtoBanco = _repository.ObterPorId(id);
            if (produtoBanco == null)
            {
                throw new DomainException("Produto nao encontrado");
            }

            if (_repository.NomeExiste(produtoDto.Nome, produtoIdAtual:id))
            {
                throw new DomainException("Ja existe outro produto com esse nome");
            }

            if (produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preco deve ser maior que zero");
            }

            produtoBanco.Nome = produtoDto.Nome;
            produtoBanco.Preco = produtoDto.Preco;
            produtoBanco.Descricao = produtoDto.Descricao;

            if(produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem);
            }

            if(produtoDto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produtoDto.StatusProduto.Value;
            }

            _repository.Atualizar(produtoBanco, produtoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produtoBanco);
        }

        public void Remover (int id)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(id);

            if(produto == null)
            {
                throw new DomainException("Produto nao encontrado");
            }

            _repository.Remover(id);
        }
    }
}
