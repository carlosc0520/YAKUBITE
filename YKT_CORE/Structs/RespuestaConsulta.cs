namespace YKT.CORE.Structs
{
    public class RespuestaEntidad<T> : RespuestaConsulta
    {
        public T Entidad { get; set; }
    }
    public class RespuestaLista<T> : RespuestaConsulta
    {
        public List<T> Lista { get; set; }
    }
    public class RespuestaConsulta
    {
        public int CodEstado { get; set; }
        public int Retorno { get; set; } = 0;
        public bool EsSatisfactoria => CodEstado >= 0;
        public string Nombre { get; set; }
        public string NombreProcedimiento { get; set; }
        public string AccessToken { get; set; } = "";
        public string Message { get; set; } = "";
        public string Mensaje// { get; set; }
        {
            get
            {
                switch (CodEstado)
                {
                    case -3:
                    case -2:
                    case -1:
                        return $"{Nombre}";
                    case 0:
                        return "No hay respuesta";
                    case 1:
                        return "No hay error";
                    default:
                        return $"{CodEstado}";
                }
            }
        }
    }
}
