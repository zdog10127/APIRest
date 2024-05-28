using APIRest.Tarefa.Domain.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRest.Tarefa.Service
{
    public class ServiceLog
    {
        private readonly string fileLogName;

        public ServiceLog()
        {
            this.fileLogName = $"logTransaction_{DateTime.Now.ToString("ddMMyyyy")}";
        }

        public async Task GravarLog(LogRequestDto logRequestDto)
        {
            string filePathLog = $"{AppContext.BaseDirectory}Logs";

            using (StreamWriter w = File.AppendText($"{filePathLog}\\{fileLogName}"))
            {
                w.WriteLine("");
                w.WriteLine($"DataLog: {DateTime.Now}");
                w.WriteLine($"TipoLog: {logRequestDto.tipoLog}");
                w.WriteLine($"Request: {logRequestDto.request}");
                w.WriteLine("");
                w.WriteLine($"Request: {logRequestDto.response}");
                if (logRequestDto.error is not null || logRequestDto.error != "")
                {
                    w.WriteLine("");
                    w.WriteLine($"Error: {logRequestDto.error}");
                }
            }
        }
    }
}
