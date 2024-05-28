using APIRest.Tarefa.Domain.Entity;
using APIRest.Tarefa.Domain.Interfaces;
using APIRest.Tarefa.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Infra.Repositories
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly Contexts _context;
        
        public RepositoryUsuario()
        {
            _context = new Contexts();
        }

        public async Task<Usuario> ListarUsuario(string email, string senha)
        {
            var usuario = await _context.Set<Usuario>().Where(x => x.Email == email && x.Senha == senha).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task GravarUsuario(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
            _context.SaveChanges();
        }

        public async Task<bool> AtualizarUsuario(int idUsuario, Usuario usuario)
        {
            var usuarioExist = await _context.Set<Usuario>().Where(x => x.UsuarioId == idUsuario).FirstOrDefaultAsync();
            if (usuarioExist == null)
                return false;

            usuarioExist.Nome = usuario.Nome;
            usuarioExist.Email = usuario.Email;
            usuarioExist.Senha = usuario.Senha;

            _context.Usuario.Update(usuarioExist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletarUsuario(int idUsuario)
        {
            var usuarioExist = await _context.Set<Usuario>().Where(x => x.UsuarioId == idUsuario).FirstOrDefaultAsync();
            if (usuarioExist == null)
                return false;

            _context.Remove(usuarioExist);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
