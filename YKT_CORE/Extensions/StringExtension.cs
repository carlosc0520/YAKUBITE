using YKT.CORE.Structs;
using System.Text.Json;

namespace YKT.CORE.Extensions
{
    public static class StringExtension
    {
        public static string AgregarParametrosDatatable(this string url, DataTablesStructs.SentParameters parameters)
        {
            var totalurl = "";
            if (url.Contains("?"))
                totalurl = $"{url}&";
            else
                totalurl = $"{url}?";
            return $"{totalurl}draw={parameters.DrawCounter}&start={parameters.PagingFirstRecord}&length={parameters.RecordsPerDraw}&order%5B0%5D%5Bcolumn%5D={parameters.OrderColumn}&order%5B0%5D%5Bdir%5D={parameters.OrderDirection}&search%5Bvalue%5D={parameters.SearchValue}";
        }
        public static string AgregarParametrosSelect2(this string url, Select2Structs.RequestParameters param)
        {
            var totalurl = "";
            if (url.Contains("?"))
                totalurl = $"{url}&";
            else
                totalurl = $"{url}?";
            return $"{totalurl}page={param.CurrentPage}&term={param.SearchTerm}";
        }
        public static RespuestaConsulta Respuesta(this HttpResponseMessage request, int codigo, string nombre)
        {
            try
            {
                if (request.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<RespuestaConsulta>(
                         (request.Content.ReadAsStringAsync()).Result,
                         new JsonSerializerOptions
                         {
                             PropertyNameCaseInsensitive = true
                         }
                     );
                }
                else
                {
                    return new RespuestaConsulta()
                    {
                        CodEstado = codigo,///-2,
                        Nombre = nombre //nameof(Insertar),
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new RespuestaConsulta()
                {
                    CodEstado = codigo,///-2,
                    Nombre = nombre, //nameof(Insertar),
                    NombreProcedimiento = ex.Message
                };
            }
        }
        public static DataTablesStructs.ReturnedData<T> RespuestaTabla<T>(this HttpResponseMessage request, int codigo)
        {
            try
            {
                if (request.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<DataTablesStructs.ReturnedData<T>>(
                        (request.Content.ReadAsStringAsync()).Result,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                }
                else
                {
                    return new DataTablesStructs.ReturnedData<T>
                    {
                        Error = "Error al cargar la tabla",
                        Data = new List<T>(),
                        DrawCounter = 1,
                        RecordsFiltered = 0,
                        RecordsTotal = 0
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new DataTablesStructs.ReturnedData<T>
                {
                    Error = "Error al cargar la tabla: " + ex.Message,
                    Data = new List<T>(),
                    DrawCounter = 1,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }
        }
    }
}
