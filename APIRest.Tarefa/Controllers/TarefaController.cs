using APIRest.Tarefa.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace APIRest.Tarefa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly IServiceTarefa _serviceTarefa;

        public TarefaController(IServiceTarefa serviceTarefa)
        {
            _serviceTarefa = serviceTarefa;
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

        [HttpGet]
        [Route("/Tarefa")]
        [ProducesResponseType(typeof(Domain.Entity.Tarefa), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarTodasTarefas()
        {
            var tarefa = await _serviceTarefa.ListarTodasTarefasAsync();

            if (tarefa.Count == 0)
            {
                ProblemDetails detalhesDoProblema = new ProblemDetails();
                detalhesDoProblema.Status = StatusCodes.Status404NotFound;
                detalhesDoProblema.Type = "NotFound";
                detalhesDoProblema.Title = "Registro não Encontrado";
                detalhesDoProblema.Detail = $"Não foram encontrados registros. ";
                detalhesDoProblema.Instance = HttpContext.Request.Path;
                return NotFound(detalhesDoProblema);
            }

            return Ok(tarefa);
        }

        [HttpGet]
        [Route("/Tarefa/{status}")]
        [ProducesResponseType(typeof(Domain.Entity.Tarefa), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTarefaPorStatus(string status)
        {
            var tarefa = await _serviceTarefa.ObterTarefaPorStatus(status);

            if (tarefa.Count == 0)
            {
                ProblemDetails detalhesDoProblema = new ProblemDetails();
                detalhesDoProblema.Status = StatusCodes.Status404NotFound;
                detalhesDoProblema.Type = "NotFound";
                detalhesDoProblema.Title = "Registro não Encontrado";
                detalhesDoProblema.Detail = $"Não foram encontrados registros. ";
                detalhesDoProblema.Instance = HttpContext.Request.Path;
                return NotFound(detalhesDoProblema);
            }

            return Ok(tarefa);
        }


        [HttpPost]
        [Route("/Tarefa")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarTarefa([FromBody] Domain.Entity.Tarefa tarefa)
        {
            if (tarefa != null)
            {
                var retornoDto = await _serviceTarefa.CriarTarefa(tarefa);

                if (retornoDto.HouveErro == true)
                    return retornoDto.RetornarResultado(HttpContext.Request.Path);
                else
                {
                    return StatusCode((int)HttpStatusCode.Created, tarefa);
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

        [Authorize]
        [HttpPut]
        [Route("/Tarefa/{idTarefa}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarTarefa([FromBody] Domain.Entity.Tarefa tarefa, int idTarefa)
        {
            var retornoDto = await _serviceTarefa.AtualizarTarefa(idTarefa, tarefa);

            if (retornoDto.HouveErro == true)
                return retornoDto.RetornarResultado(HttpContext.Request.Path);
            else
            {
                return StatusCode((int)HttpStatusCode.OK, tarefa);
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("/Tarefa/{idTarefa}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarTarefa(int idTarefa)
        {
            var retornoDto = await _serviceTarefa.DeletarTarefa(idTarefa);

            if (retornoDto.HouveErro == true)
                return retornoDto.RetornarResultado(HttpContext.Request.Path);
            else
            {
                return StatusCode((int)HttpStatusCode.OK, idTarefa);
            }
        }
    }
}
