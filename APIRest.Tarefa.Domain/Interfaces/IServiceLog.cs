using APIRest.Tarefa.Domain.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Domain.Interfaces
{
    public interface IServiceLog
    {
        Task GravarLog(LogRequestDto logRequestDto);
    }
}
