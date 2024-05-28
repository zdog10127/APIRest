namespace APIRest.Tarefa.Domain.Interfaces
{
    public interface IRepositoryTarefa
    {
        Task<List<Entity.Tarefa>> ListarTodasTarefasAsync();
        Task<List<Entity.Tarefa>> ObterTarefaPorStatus(string status);
        Task CriarTarefa(Entity.Tarefa tarefa);
        Task<bool> AtualizarTarefa(int idTarefa, Entity.Tarefa tarefa);
        Task<bool> DeletarTarefa(int idTarefa);
    }
}
