namespace YKT.CONFIG
{
    public class ConfiguracionProyecto
    {
        public const string PROYECTO = PROYECTOS.Caro;
        public const string TEMA = TEMAS.Default;
        public const string ENTORNO_DESPLIEGUE = ENTORNOS.Desarrollo;

        public const int TIEMPO_SESION_MINUTOS = 120;

        public const string DISK = "C:\\JAKUBITE\\";
        public const string HOST = "";

        //public const string DISK = "h:\\root\\home\\devcar0520-001\\www\\";
        //public const string HOST = "http://devcar0520-001-site11.etempurl.com/";


        //public const string DISKFILE = "http://devcar0520-001-site13.etempurl.com/";
        public static class TEMAS
        {
            public const string Default = "default";
        }
        public static class PROYECTOS
        {
            public const string Caro = "CARO";
        }
        public static class ENTORNOS
        {
            public const string Desarrollo = "Development";
            public const string Pruebas = "Staging";
            public const string Produccion = "Production";
        }

        public static class MODULOS
        {

        }
        public static class CARPETAS_DESPLIEGUE
        {
            public const string Login_Web = "login";
            public const string Seguridad_Web = "sgs-web";
            public const string Persona_Web = "per-web";
        }

        public static class CADENA_CONEXION
        {
            public const string Seguridad = "DefaultConnection";
            public const string Persona = "DefaultConnection";
        }

        public static class CAPTCHA
        {
            public const string SecretKey = "6Lcd6LAmAAAAABgcM7kchoISw0lw9TADkxGdSsLL";
            public const decimal ScoreAdmitido = 0.5M;
        }

        public static class APIS
        {

        }

        public static class TOKEN
        {

        }
         
        public static class CORREOS_CONTACTO
        {
            public const string CORREO = "ccarbajalmt0520@gmail.com";
            public const string KEY = "qcigvfwwdyrwelib";
        }
    }
}
