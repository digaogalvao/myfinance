using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : ControllerBase
    {
        private readonly ILancamentoService _lancamentoService;

        public LancamentosController(ILancamentoService lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lancamento>>> Index()
        {
            var lancamentos = await _lancamentoService.GetLancamentos();

            if (lancamentos is null)
                return NotFound($"Nenhum lançamento cadastrado");

            return Ok(lancamentos);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Lancamento>> Details(int id)
        {
            var lancamento = await _lancamentoService.GetLancamentoById(id);

            if (lancamento is null)
                return NotFound($"Lançamento com id= {id} não encontrado");

            return Ok(lancamento);
        }

        [Authorize]
        [HttpGet("user/{id:int}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> DetailsByUser(int id)
        {
            var lancamentos = await _lancamentoService.GetLancamentosByUsuario(id);

            if (lancamentos is null)
                return NotFound($"Lançamento com id= {id} de usuário não encontrado");

            return Ok(lancamentos);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] Lancamento lancamento)
        {
            try
            {
                _lancamentoService.CreateLancamento(lancamento);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Lancamento lancamento)
        {
            try
            {
                if (lancamento.Id != id)
                    return NotFound($"Lançamento com id= {id} não encontrado");

                await _lancamentoService.UpdateLancamento(lancamento);

                // Buscar os dados atualizados
                var lancamentoAtualizado = await _lancamentoService.GetLancamentoById(id);

                if (lancamentoAtualizado == null)
                    return NotFound($"Lançamento com id= {id} não encontrado após atualização");

                return Ok(lancamentoAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var lancamento = await _lancamentoService.GetLancamentoById(id);

                if (lancamento is null)
                    return NotFound($"Lançamento com id= {id} não encontrado");

                await _lancamentoService.DeleteLancamento(id);
                return Ok(lancamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("fluxo-caixa")]
        public async Task<ActionResult<FluxoCaixa>> GetFluxoCaixa([FromQuery] int usuarioId, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] int? mes, [FromQuery] int? ano)
        {
            var resumo = await _lancamentoService.GetFluxoCaixa(usuarioId, dataInicial, dataFinal, mes, ano);
            return Ok(resumo);
        }
    }
}
