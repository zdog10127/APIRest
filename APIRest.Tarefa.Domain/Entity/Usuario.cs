using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Domain.Entity
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Required]
        [Column("UsuarioID", TypeName = "int")]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Nome", TypeName = "varchar(50)")]
        public string Nome { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Email", TypeName = "varchar(200)")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Senha", TypeName = "varchar(100)")]
        public string Senha { get; set; } 
    }
}
