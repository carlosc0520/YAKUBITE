using Dapper;
using YKT.CORE.Structs;
using YKT.DATABASE.Configuracion;
using YKT.DATABASE.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace YKT.DATABASE
{
    public static class FuncionesSql
    {
        public static async Task<List<T>> EjecutarProcedimiento<T>(string? conexionSql, string nombreProcedimiento, DynamicParameters parametros)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    //reader = await conn.ExecuteReaderAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var enumerable = await conn.QueryAsync<T>(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    return enumerable.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }

        public static async Task<T> EjecutarProcedimientoFirst<T>(string? conexionSql, string nombreProcedimiento, DynamicParameters parametros)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    //reader = await conn.ExecuteReaderAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var enumerable = await conn.QueryAsync<T>(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    return enumerable.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<RespuestaLista<T>> EjecutarProcedimientoRespuesta<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    //reader = await conn.ExecuteReaderAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var enumerable = await conn.QueryAsync<T>(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    return new RespuestaLista<T>
                    {
                        CodEstado = 1,
                        Lista = enumerable.ToList(),
                    };
                }
                catch (Exception ex)
                {
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(typeof(T));

                    var instance = (List<T>)Activator.CreateInstance(constructedListType);
                    return new RespuestaLista<T>
                    {
                        CodEstado = -1,
                        Lista = instance,
                        Nombre = ex.Message
                    };
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<T> EjecutarFuncion<T>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    var paramet = "";
                    var paramet2 = "";
                    //var paramet = "";
                    if (parametros != null)
                    {
                        paramet = "{";
                        var items = new List<string>();
                        foreach (var item in parametros?.ParameterNames)
                        {
                            var val = parametros.Get<dynamic>(item);
                            items.Add($"{val}");
                        }
                        paramet += string.Join(",", items) + "}";
                        paramet2 = string.Join(",", items.Select(x => $"{x}"));
                    }
                    //object obj = null;
                    //if (!string.IsNullOrEmpty(paramet2))
                    //{
                    //    obj = JsonSerializer.Deserialize<object>(paramet);
                    //}
                    var res = await conn.ExecuteScalarAsync<T>($"SELECT {nombreProcedimiento}({paramet2})", commandType: CommandType.Text);
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<RespuestaConsulta> EjecutarProcedimiento(string conexionSql, string nombreProcedimiento, string variableRetorno, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    await conn.ExecuteAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var vRetorno = parametros.Get<int>(variableRetorno);
                    return new RespuestaConsulta()
                    {
                        CodEstado = vRetorno,
                        NombreProcedimiento = nombreProcedimiento,
                        Nombre = "Base de Datos"
                    };
                }
                catch (Exception ex)
                {
                    return new RespuestaConsulta()
                    {
                        CodEstado = -1,
                        NombreProcedimiento = nombreProcedimiento,
                        Nombre = $"{ex.Message}"
                    };
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<RespuestaConsulta> EjecutarProcedimiento(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    await conn.ExecuteAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    
                    var vRetorno = parametros.Get<int>("@p_nId");
                    return new RespuestaConsulta()
                    {
                        CodEstado = 1,
                        Retorno = vRetorno,
                        NombreProcedimiento = nombreProcedimiento,
                        Nombre = "Base de Datos"
                    };
                }
                catch (Exception ex)
                {
                    return new RespuestaConsulta()
                    {
                        CodEstado = -1,
                        NombreProcedimiento = nombreProcedimiento,
                        Nombre = $"{ex.Message}"
                    };
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<T> EjecutarProcedimiento<T>(string conexionSql, string nombreProcedimiento, string variableRetorno, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    await conn.ExecuteAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var vRetorno = parametros.Get<T>(variableRetorno);
                    return vRetorno;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<RespuestaEntidad<T>> ObtenerPrimerRegistro<T>(string? conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    var enumerable = await conn.QueryAsync<T>(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);

                    return new RespuestaEntidad<T>()
                    {
                        Entidad = enumerable.FirstOrDefault(),
                        CodEstado = 1,
                        NombreProcedimiento = nombreProcedimiento,
                    };
                }
                catch (Exception ex)
                {
                    return new RespuestaEntidad<T>()
                    {
                        Entidad = (T)Activator.CreateInstance(typeof(T)),
                        CodEstado = -1,
                        NombreProcedimiento = nombreProcedimiento,
                        Nombre = $"Base de Datos - {ex.Message}"
                    };
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<Cursores<T1, T2>> ObtenerCursores<T1, T2>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    var multi = await conn.QueryMultipleAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);

                    return new Cursores<T1, T2>()
                    {
                        Cursor1 = multi.Read<T1>().ToList(),
                        Cursor2 = multi.Read<T2>().ToList()
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<Cursores<T1, T2, T3>> ObtenerCursores<T1, T2, T3>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    var multi = await conn.QueryMultipleAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);

                    return new Cursores<T1, T2, T3>()
                    {
                        Cursor1 = multi.Read<T1>().ToList(),
                        Cursor2 = multi.Read<T2>().ToList(),
                        Cursor3 = multi.Read<T3>().ToList(),
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<CursorEntidad<T1, T2>> ObtenerCursorEntidad<T1, T2>(string conexionSql, string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (var conn = new SqlConnection(conexionSql))
            {
                try
                {
                    conn.Open();
                    var multi = await conn.QueryMultipleAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);

                    return new CursorEntidad<T1, T2>()
                    {
                        Cursor1 = multi.Read<T1>().ToList().FirstOrDefault(),
                        Cursor2 = multi.Read<T2>().ToList().FirstOrDefault()
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }
        public static async Task<List<T>> EjecutarQuery<T>(ConfiguracionConexionBDLogueo conexionSql, string query)
        {
            using (var conn = new SqlConnection(conexionSql.CadenaConexion))
            {
                try
                {
                    conn.Open();
                    //reader = await conn.ExecuteReaderAsync(nombreProcedimiento, parametros, commandType: CommandType.StoredProcedure);
                    var enumerable = await conn.QueryAsync<T>(query);
                    return enumerable.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    SqlConnection.ClearAllPools();
                }
            }
        }

       
    }
}
