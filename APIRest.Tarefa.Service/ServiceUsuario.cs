using APIRest.Tarefa.Domain.Dto;
using APIRest.Tarefa.Domain.Entity;
using APIRest.Tarefa.Domain.Interfaces;
using APIRest.Tarefa.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Service
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public ServiceUsuario()
        {
            _repositoryUsuario = new RepositoryUsuario();
        }

        public async Task<Usuario> ListarUsuario(string email, string senha)
        {
            var usuario = await _repositoryUsuario.ListarUsuario(email, senha);
            return usuario;
        }

        public async Task<RetornoDto> GravarUsuario(Usuario usuario)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryUsuario.GravarUsuario(usuario);

            if (ret.Exception != null) 
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Gravar Usuario";
                retornoDto.MensagemErro = "Erro ao gravar o usuario";
            }

            return await Task.FromResult(retornoDto);
        }

        public async Task<RetornoDto> AtualizarUsuario(int idUsuario, Usuario usuario)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryUsuario.AtualizarUsuario(idUsuario, usuario);

            if (ret.Exception != null)
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Atualizar Usuario";
                retornoDto.MensagemErro = "Erro ao atualizar o usuario";
            }

            return await Task.FromResult(retornoDto);
        }

        public async Task<RetornoDto> DeletarUsuario(int idUsuario)
        {
            RetornoDto retornoDto = new RetornoDto();

            var ret = _repositoryUsuario.DeletarUsuario(idUsuario);

            if (ret.Exception != null)
            {
                retornoDto.HouveErro = true;
                retornoDto.CodigoErro = "400";
                retornoDto.TituloErro = "Deletar Usuario";
                retornoDto.MensagemErro = "Erro ao deletar o usuario";
            }

            return await Task.FromResult(retornoDto);
        }
    }
}
