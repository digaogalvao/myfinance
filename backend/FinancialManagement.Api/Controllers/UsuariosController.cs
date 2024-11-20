using FinancialManagement.Api.ViewModels;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public UsuariosController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Index()
        {
            var usuarios = await _usuarioService.GetUsuarios();

            if (usuarios is null)
                return NotFound($"Nenhum usuário cadastrado");

            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> Details(int id)
        {
            var usuario = await _usuarioService.GetUsuario(id);

            if (usuario is null)
                return NotFound($"Usuário com id= {id} não encontrado");

            return Ok(usuario);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel usuario)
        {
            var login = await _usuarioService.LoginUsuario(usuario.Email, usuario.Senha);

            if (login is null)
                return Unauthorized("Email ou senha inválidos");

            var token = _tokenService.GenerateToken(login);

            return Ok(new LoginResponseViewModel
            {
                Token = token,
                UserId = login.Id,
                UserName = login.Nome,
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            try
            {
                await _usuarioService.CreateUsuario(usuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
