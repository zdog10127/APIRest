using APIRest.Tarefa.Domain.Dto;

namespace APIRest.Tarefa.Domain.Interfaces
{
    public interface IServiceTarefa
    {
        Task<List<Entity.Tarefa>> ListarTodasTarefasAsync();
        Task<List<Entity.Tarefa>> ObterTarefaPorStatus(string status);
        Task<RetornoDto> CriarTarefa(Entity.Tarefa tarefa);
        Task<RetornoDto> AtualizarTarefa(int idTarefa, Entity.Tarefa tarefa);
        Task<RetornoDto> DeletarTarefa(int idTarefa);
    }
}
