using APIRest.Tarefa.Domain.Dto;
using APIRest.Tarefa.Domain.Entity;
using APIRest.Tarefa.Domain.Interfaces;
using APIRest.Tarefa.Infra.Context;
using APIRest.Tarefa.Service;
using APIRest.Tarefa.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace APIRest.Tarefa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario _serviceUsuario;
        private readonly Contexts _context;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
            _context = new Contexts();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HealthCheck()
        {
            StringBuilder informacoes = new StringBuilder();
            informacoes.AppendLine($"APIRestTarefa = APIRest.Tarefa");
            informacoes.AppendLine($"Situação = Saudável");

            return Ok(informacoes.ToString());
        }

        [HttpPost]
        [Route("/Usuario/{email}/{senha}")]
        [ProducesResponseType(typeof(Domain.Entity.Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarUsuario(string email, string senha)
        {
            var usuario = await _serviceUsuario.ListarUsuario(email, senha);

            if (usuario is null)
            {
                ProblemDetails detalhesDoProblema = new ProblemDetails();
                detalhesDoProblema.Status = StatusCodes.Status404NotFound;
                detalhesDoProblema.Type = "NotFound";
                detalhesDoProblema.Title = "Registro não Encontrado";
                detalhesDoProblema.Detail = $"Não foram encontrados registros. ";
                detalhesDoProblema.Instance = HttpContext.Request.Path;
                return NotFound(detalhesDoProblema);
            }

            var auth = Authenticate(email, senha);

            return Ok(auth);
        }

        [HttpPost]
        [Route("/Usuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GravarUsuario([FromBody] Domain.Entity.Usuario usuario)
        {
            if (usuario != null)
            {
                var retornoDto = await _serviceUsuario.GravarUsuario(usuario);

                if (retornoDto.HouveErro == true)
                    return retornoDto.RetornarResultado(HttpContext.Request.Path);
                else
                {
                    return StatusCode((int)HttpStatusCode.Created, usuario);
                }
            }
            else
            {
                ProblemDetails detalhesDoProblema = new ProblemDetails();
                detalhesDoProblema.Status = StatusCodes.Status400BadRequest;
                detalhesDoProblema.Type = "BadRequest";
                detalhesDoProblema.Title = "Registro não pode ser nulo";
                detalhesDoProblema.Detail = $"Dados não podem ser vazio ou nulo. ";
                detalhesDoProblema.Instance = HttpContext.Request.Path;
                return BadRequest(detalhesDoProblema);
            }
        }

        [HttpPut]
        [Route("/Usuario/{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarUsuario([FromBody] Domain.Entity.Usuario usuario, int idUsuario)
        {
            var retornoDto = await _serviceUsuario.AtualizarUsuario(idUsuario, usuario);

            if (retornoDto.HouveErro == true)
                return retornoDto.RetornarResultado(HttpContext.Request.Path);
            else
            {
                return StatusCode((int)HttpStatusCode.OK, usuario);
            }

        }

        [HttpDelete]
        [Route("/Usuario/{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarUsuario(int idUsuario)
        {
            var retornoDto = await _serviceUsuario.DeletarUsuario(idUsuario);

            if (retornoDto.HouveErro == true)
                return retornoDto.RetornarResultado(HttpContext.Request.Path);
            else
            {
                return StatusCode((int)HttpStatusCode.OK, idUsuario);
            }
        }

        private UserToken Authenticate(string email, string password)
        {
            var userToken = new UserToken();

            var user = _context.Set<Usuario>().Where(x => x.Email == email && x.Senha == password).FirstOrDefault();
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigAppSettings.ConfiguracaoJWT());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.Nome)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userToken.Token = tokenHandler.WriteToken(token);

            return userToken;
        }
    }
}
