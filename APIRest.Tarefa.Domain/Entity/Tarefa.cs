using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIRest.Tarefa.Domain.Entity
{
    [Table("Tarefa")]
    public class Tarefa
    {
        [Key]
        [Required]
        [Column("TarefaID", TypeName = "int")]
        public int TarefaId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Titulo", TypeName = "varchar(50)")]
        public string Titulo { get; set; }

        [Required]
        [MaxLength(250)]
        [Column("Descricao", TypeName = "varchar(250)")]
        public string Descricao { get; set; }

        [Required]
        [Column("DataCriacao", TypeName = "datetime")]
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Status
        /// 1 = Pendente;
        /// 2 = Em andamento;
        /// 3 = Concluída;
        /// </summary>
        [Required]
        [Column("Status", TypeName = "int")]
        public int Status { get; set; }
    }
}
