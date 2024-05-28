using APIRest.Tarefa.Domain.Dto;
using APIRest.Tarefa.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Domain.Interfaces
{
    public interface IServiceUsuario
    {
        Task<Usuario> ListarUsuario(string email, string senha);
        Task<RetornoDto> GravarUsuario(Usuario usuario);
        Task<RetornoDto> AtualizarUsuario(int idUsuario, Usuario usuario);
        Task<RetornoDto> DeletarUsuario(int idUsuario);
    }
}
