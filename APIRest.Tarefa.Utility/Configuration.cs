using Microsoft.Extensions.Configuration;

namespace APIRest.Tarefa.Utility
{
    internal class Configuration
    {
        private readonly IConfiguration configAppSettings;

        public IConfiguration ConfiguracaoAppSettings
        {
            get { return this.configAppSettings; }
        }

        internal Configuration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
                throw new ArgumentNullException("A variável de ambiente ASPNETCORE_ENVIRONMENT não está configurada");

            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false).AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false).AddEnvironmentVariables();
                this.configAppSettings = builder.Build();
            }
            catch (FormatException e)
            {
                throw new FormatException("Arquivos de configuração não configurados corretamente! Verifique o formato do arquivo .json", e);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao carregar arquivos de configuração", e);
            }
        }
    }
}
