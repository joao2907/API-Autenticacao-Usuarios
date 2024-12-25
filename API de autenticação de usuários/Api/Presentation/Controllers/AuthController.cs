using Api.Application.DTOs;
using Api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto loginDto)
        {
            try
            {
                await _userService.RegisterAsync(loginDto);
                return Ok(new { message = "Usuário cadastrado com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                // Retorna o código 400 para erro de validação
                return BadRequest(new { message = ex.Message }); // Aqui retorna a mensagem do erro
            }
            catch (Exception ex)
            {
                // Retorna o código 500 para erros inesperados
                return StatusCode(500, new { message = "Ocorreu um erro inesperado. Tente novamente mais tarde." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Autentica o usuário
                var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

                // Gera o token JWT
                var token = await _userService.GenerateJwtAsync(user.Email, user.Name);

                // Retorna apenas o token
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Retorna o código 400 para erro de validação
                return BadRequest(new { message = ex.Message }); // Aqui retorna a mensagem do erro
            }
            catch (Exception ex)
            {
                // Retorna o código 500 para erros inesperados
                return StatusCode(500, new { message = "Ocorreu um erro inesperado. Tente novamente mais tarde." });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Verificar se o token foi informado no cabeçalho
                if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrWhiteSpace(Request.Headers["Authorization"]))
                {
                    return BadRequest(new { message = "Token de autorização não fornecido." });
                }

                // Extrai o token do cabeçalho
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Este serviço faz a lógica de revogar o token, adicionando-o à lista de tokens revogados no banco de dados para evitar seu uso futuro.
                await _userService.LogoutAsync(token);

                // Isso informa ao cliente que o token foi revogado e não pode mais ser usado.
                return Ok(new { message = "Logout realizado com sucesso." });
            }
            catch (Exception ex)
            {
                // Retorna um código HTTP 500 "Internal Server Error" para erros inesperados.
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}