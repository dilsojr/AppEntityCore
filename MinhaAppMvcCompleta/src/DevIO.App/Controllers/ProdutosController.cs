using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;

namespace DevIO.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IFornecedorRepository fornecedorRepository;
        private readonly IMapper mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            this.produtoRepository = produtoRepository;
            this.fornecedorRepository = fornecedorRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<IEnumerable<ProdutoViewModel>>(await produtoRepository.ObterProdutosFornecedores()));
        }

        public async Task<IActionResult> Details(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        public  async Task<IActionResult>Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            if (!ModelState.IsValid)
                return View(produtoViewModel);

            await produtoRepository.Adicionar(mapper.Map<Produto>(produtoViewModel));

            return View(produtoViewModel);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            
            var produtoViewModel = await  ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(produtoViewModel);

            await produtoRepository.Atualizar(mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var produtoViewModel = ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }


        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = mapper.Map<ProdutoViewModel>(await produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = mapper.Map<IEnumerable<FornecedorViewModel>>(await fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel.Fornecedores = mapper.Map<IEnumerable<FornecedorViewModel>>(await fornecedorRepository.ObterTodos());
            return produtoViewModel;
        }

    }
}
