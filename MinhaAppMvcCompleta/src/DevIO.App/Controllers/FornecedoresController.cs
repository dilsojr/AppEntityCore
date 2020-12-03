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

    public class FornecedoresController : BaseController
    {

        private readonly IFornecedorRepository fornecedorRepository;
        private readonly IMapper mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            this.fornecedorRepository = fornecedorRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<IEnumerable<FornecedorViewModel>>(await fornecedorRepository.ObterTodos()));
        }

        public async Task<IActionResult> Details(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = mapper.Map<Fornecedor>(fornecedorViewModel);
            await fornecedorRepository.Adicionar(fornecedor);
            

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutpsEndereco(id);
          
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            
            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();
            
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = mapper.Map<Fornecedor>(fornecedorViewModel);
            await fornecedorRepository.Atualizar(fornecedor);

            return RedirectToAction(nameof(Index));
           
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
           
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await fornecedorRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return mapper.Map<FornecedorViewModel>(await fornecedorRepository.OberFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutpsEndereco(Guid id)
        {
            return mapper.Map<FornecedorViewModel>(await fornecedorRepository.ObterFornecedorProdutoEndereco(id));
        }
    }
}
