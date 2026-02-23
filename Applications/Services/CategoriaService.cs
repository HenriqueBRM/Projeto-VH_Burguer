using Microsoft.Identity.Client;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.CategoriaDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<LerCategoriaDto> Listar()
        {
            List<Categoria> categorias = _repository.Listar();

            List<LerCategoriaDto> categoriaDto = categorias.Select(categoria => new LerCategoriaDto
            {
                CategoriaID = categoria.CategoriaID,
                Nome = categoria.Nome
            }).ToList();

            // Retorna a lista em Dto
            return categoriaDto;
        }

        public LerCategoriaDto ObterPorId(int id)
        {
            Categoria categoria = _repository.ObterPorId(id);
            if (categoria == null)
            {
                throw new DomainException("Categoria nao encontrada");
            }

            LerCategoriaDto categoriaDto = new LerCategoriaDto
            {
                CategoriaID = categoria.CategoriaID,
                Nome = categoria.Nome
            };

            return categoriaDto;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome eh obrigatorio");
            }
        }

        public void Adicionar(CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Categoria ja existente");
            }

            Categoria categoria = new Categoria
            {
                Nome = criarDto.Nome,
            };

            _repository.Adicionar(categoria);
        }

        public void Atualizar(int id, CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null)
            {
                throw new DomainException("Categoria nao encontrada");
            }

            if (_repository.NomeExiste(criarDto.Nome, categoriaIdAtual: id))
            {
                throw new DomainException("Ja existe outra categoria com esse nome");
            }

            categoriaBanco.Nome = criarDto.Nome;
            _repository.Atualizar(categoriaBanco);
        }

        public void Remover(int id)
        {
            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null)
            {
                throw new DomainException("Categoria nao encontrada");
            }

            _repository.Remover(id);
        }
    }
}
