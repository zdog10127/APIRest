using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Domain.Dto.Request
{
    public class LogRequestDto
    {
        public string? tipoLog { get; set; }
        public DateTime dataLog { get; set; }
        public string? response { get; set; }
        public string? request { get; set; }
        public string? error { get; set; }

    }
}
