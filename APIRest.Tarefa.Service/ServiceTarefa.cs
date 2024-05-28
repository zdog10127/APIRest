using APIRest.Tarefa.Domain.Dto;
using APIRest.Tarefa.Domain.Interfaces;
using APIRest.Tarefa.Infra.Repositories;

namespace APIRest.Tarefa.Service
{
    public class ServiceTarefa : IServiceTarefa
    {
        private readonly IRepositoryTarefa _repositoryTarefa;

        public ServiceTarefa()
        {
            _repositoryTarefa = new RepositoryTarefa();
        }

        public async Task<List<Domain.Entity.Tarefa>> ListarTodasTarefasAsync()
        {
            var list = await _repositoryTarefa.ListarTodasTarefasAsync();
            return list;
        }

        public async Task<List<Domain.Entity.Tarefa>> ObterTarefaPorStatus(string status)
        {
            var listStatus = await _repositoryTarefa.ObterTarefaPorStatus(status);
            return listStatus;
        }

        public async Task<RetornoDto> CriarTarefa(Domain.Entity.Tarefa tarefa)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryTarefa.CriarTarefa(tarefa);

            if (ret.Exception != null)
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Criar Tarefa";
                retornoDto.MensagemErro = "Erro ao criar a tarefa";
            }

            return await Task.FromResult(retornoDto);
        }

        public async Task<RetornoDto> AtualizarTarefa(int idTarefa, Domain.Entity.Tarefa tarefa)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryTarefa.AtualizarTarefa(idTarefa, tarefa);

            if (ret.Exception != null)
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Atualizar Tarefa";
                retornoDto.MensagemErro = "Erro ao atualizar a tarefa";
            }

            return await Task.FromResult(retornoDto);
        }

        public async Task<RetornoDto> DeletarTarefa(int idTarefa)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryTarefa.DeletarTarefa(idTarefa);

            if (ret.Exception != null)
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Deletar Tarefa";
                retornoDto.MensagemErro = "Erro ao deletar a tarefa";
            }

            return await Task.FromResult(retornoDto);
        }
    }
}
