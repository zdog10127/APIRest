using APIRest.Tarefa.Domain.Interfaces;
using APIRest.Tarefa.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Tarefa.Infra.Repositories
{
    public class RepositoryTarefa : IRepositoryTarefa
    {
        private readonly Contexts _context;

        public RepositoryTarefa()
        {
            _context = new Contexts();
        }

        public async Task<List<Domain.Entity.Tarefa>> ListarTodasTarefasAsync()
        {
            var list = await _context.Tarefa.ToListAsync();
            return list;
        }

        public async Task<List<Domain.Entity.Tarefa>> ObterTarefaPorStatus(string status)
        {
            string status1 = "Pendente";
            string status2 = "Em andamento";
            string status3 = "Concluída";

            var lstTarefa = new List<Domain.Entity.Tarefa>();

            if (status1 == status)
            {
                lstTarefa = await _context.Set<Domain.Entity.Tarefa>().Where(x => x.Status == 1).ToListAsync();
            }
            else if (status2 == status)
            {
                lstTarefa = await _context.Set<Domain.Entity.Tarefa>().Where(x => x.Status == 2).ToListAsync();
            }
            else if (status3 == status)
            {
                lstTarefa = await _context.Set<Domain.Entity.Tarefa>().Where(x => x.Status == 3).ToListAsync();
            }

            return lstTarefa;
        }

        public async Task CriarTarefa(Domain.Entity.Tarefa tarefa)
        {
            await _context.Tarefa.AddAsync(tarefa);
            _context.SaveChanges();
        }

        public async Task<bool> AtualizarTarefa(int idTarefa, Domain.Entity.Tarefa tarefa)
        {
            var tarefaExist = await _context.Set<Domain.Entity.Tarefa>().Where(x => x.TarefaId == idTarefa).FirstOrDefaultAsync();
            if (tarefaExist == null)
                return false;

            tarefaExist.Titulo = tarefa.Titulo;
            tarefaExist.Descricao = tarefa.Descricao;
            tarefaExist.Status = tarefa.Status;

            _context.Update(tarefaExist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletarTarefa(int idTarefa)
        {
            var tarefaExist = await _context.Set<Domain.Entity.Tarefa>().Where(x => x.TarefaId == idTarefa).FirstOrDefaultAsync();
            if (tarefaExist == null)
                return false;

            _context.Remove(tarefaExist);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
