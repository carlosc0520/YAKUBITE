namespace YKT.DATABASE.Helper
{
    public static class Procedimientos
    {

        public static class SEGURIDAD
        {
            public const string CrudRegister = "SEG.USP_CRUD_USUARIOS";
            public const string CrudCombos = "SEG.USP_CRUD_COMBOS";
            public const string GrupoDatoCrud = "SEG.USP_CRUD_MSTB05";
        }


        public static class ADMIN
        {
            public const string CrudRestaurant = "ADM.USP_CRUD_RESTAURANT";
            public const string CrudMenu = "ADM.USP_CRUD_MENU";
            public const string CrudUsuario = "ADM.USP_CRUD_USUARIOS_ADMIN";
            public const string CrudComunidad = "ADM.USP_CRUD_FOROS";
        }

        public static class CLIENTE
        {
            public const string CrudComentarios = "CLI.USP_CRUD_COMENTARIOS";
        }
    }
}
