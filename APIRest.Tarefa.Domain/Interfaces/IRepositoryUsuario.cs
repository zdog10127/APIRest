using APIRest.Tarefa.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Domain.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<Usuario> ListarUsuario(string email, string senha);
        Task GravarUsuario(Usuario usuario);
        Task<bool> AtualizarUsuario(int idUsuario, Usuario usuario);
        Task<bool> DeletarUsuario(int idUsuario);
    }
}
