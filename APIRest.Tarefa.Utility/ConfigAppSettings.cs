using System.Net.NetworkInformation;

namespace APIRest.Tarefa.Utility
{
    public class ConfigAppSettings
    {
        public static string StringConnection()
        {
            Configuration config = new Configuration();
            return config.ConfiguracaoAppSettings["ConnectionStringSql"];
        }

        public static string ConfiguracaoJWT()
        {
            Configuration config = new Configuration();
            return config.ConfiguracaoAppSettings["JWT:key"];
        }
    }
}
