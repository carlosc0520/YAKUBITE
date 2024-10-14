using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YKT.DATABASE.Configuracion
{
    public class ConfigConexionBD
    {
        public ConfigConexionBD(string value) => CadenaConexion = value;
        public string CadenaConexion { get; }
    }
    public class ConfiguracionConexionBDLogueo : ConfigConexionBD
    {
        public ConfiguracionConexionBDLogueo(string value) : base(value)
        {
        }
    }
}
