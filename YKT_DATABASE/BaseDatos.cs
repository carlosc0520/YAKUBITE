using Dapper;
using YKT.CORE.Structs;
using YKT.DATABASE.Configuracion;
using YKT.DATABASE.Helper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YKT.DATABASE
{
    public static class BaseDatos
    {
        //aqui seleccionamos la BD que estamos utilizando o que vamos a utilizar
        public const int Actual = Posibles.SQL;
        public static class Posibles
        {
            public const int SQL = 0;
            public const int MYSQL = 1;
            public const int Oracle = 2;
        }

        public static async Task<List<T>> EjecutarProcedimiento<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<RespuestaLista<T>> EjecutarProcedimientoRespuesta<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarProcedimientoRespuesta<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<T> EjecutarFuncion<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarFuncion<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
        }
        //public static async Task<int> EjecutarProcedimiento(this ConfiguracionConexionBD conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        //{
        //    if (BaseDatos.Actual == Posibles.SQL)
        //        return await FuncionesSql.EjecutarProcedimiento(conexionSql, nombreProcedimiento, parametros);
        //    if (BaseDatos.Actual == Posibles.MYSQL)
        //        return await FuncionesMySql.EjecutarProcedimiento(conexionSql, nombreProcedimiento, parametros);
        //    if (BaseDatos.Actual == Posibles.Oracle)
        //        return await FuncionesOracle.EjecutarProcedimiento(conexionSql, nombreProcedimiento, parametros);
        //}
        public static async Task<T> EjecutarProcedimiento<T>(string conexionSql, string nombreProcedimiento, string variableRetorno, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, variableRetorno, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, variableRetorno, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, variableRetorno, parametros);
        }
        public static async Task<RespuestaConsulta> EjecutarProcedimiento(string conexionSql, string nombreProcedimiento, string variableRetorno, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarProcedimiento(conexionSql, nombreProcedimiento, variableRetorno, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, variableRetorno, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, variableRetorno, parametros);
        }

        public static async Task<RespuestaEntidad<T>> ObtenerPrimerRegistro<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.ObtenerPrimerRegistro<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.ObtenerPrimerRegistro<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.ObtenerPrimerRegistro<T>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<Cursores<T1, T2>> ObtenerCursores<T1, T2>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.ObtenerCursores<T1, T2>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.ObtenerCursores<T1, T2>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.ObtenerCursores<T1, T2>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<CursorEntidad<T1, T2>> ObtenerCursorEntidad<T1, T2>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.ObtenerCursorEntidad<T1, T2>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.ObtenerCursores<T1, T2>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.ObtenerCursores<T1, T2>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<Cursores<T1, T2, T3>> ObtenerCursores<T1, T2, T3>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.ObtenerCursores<T1, T2, T3>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.ObtenerCursores<T1, T2, T3>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.ObtenerCursores<T1, T2, T3>(conexionSql, nombreProcedimiento, parametros);
        }
        public static async Task<List<T>> EjecutarQuery<T>(this ConfiguracionConexionBDLogueo conexionSql, string query)
        {
            if (BaseDatos.Actual == Posibles.SQL)
                return await FuncionesSql.EjecutarQuery<T>(conexionSql, query);
            //if (BaseDatos.Actual == Posibles.MYSQL)
            //    return await FuncionesMySql.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
            //if (BaseDatos.Actual == Posibles.Oracle)
            //    return await FuncionesOracle.EjecutarProcedimiento<T>(conexionSql, nombreProcedimiento, parametros);
        }
    }
}
