using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Models;

namespace DevIO.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>();
            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<Endereco, EnderecoViewModel>();
        }
    }
}
