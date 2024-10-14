using YKT.CORE.Structs;
using YKT.CORE.Helpers;

namespace YKT.CORE.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> OrdenarTabla<T>(this IEnumerable<T> lista, DataTablesStructs.SentParameters parameters, Func<T, dynamic> ordernarPor)
        {
            //ascendente
            if (parameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            {
                lista = lista.OrderBy(ordernarPor);
            }
            else//descendente
            {
                lista = lista.OrderByDescending(ordernarPor);
            }
            return lista;
        }
        public static DataTablesStructs.ReturnedData<T> ConvertirTabla<T>(this IEnumerable<T> lista, DataTablesStructs.SentParameters parameters, Func<T, dynamic> ordernarPor = null)
        {
            try
            {
                if (ordernarPor != null)
                {
                    lista = lista.OrdenarTabla(parameters, ordernarPor);
                }

                var recordsFiltered = lista.Count();

                var data = lista
                    .Skip(parameters.PagingFirstRecord)
                    .Take(parameters.RecordsPerDraw)
                    .ToList();

                var recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<T>
                {
                    Data = data,
                    DrawCounter = parameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
            catch (Exception ex)
            {
                return new DataTablesStructs.ReturnedData<T>
                {
                    Error = ex.Message,
                    Data = new List<T>(),
                    DrawCounter = 1,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }
        }
        public static Select2Structs.ResponseParameters ConvertirSelect2<T>(this IEnumerable<T> lista, Select2Structs.RequestParameters parameters, Func<T, Select2Structs.Result> seleccionar)
        {
            try
            {
                var query = lista;

                var currentPage = parameters.CurrentPage != 0 ? parameters.CurrentPage - 1 : 0;

                var results = query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(seleccionar)
                .ToList();

                return new Select2Structs.ResponseParameters
                {
                    Pagination = new Select2Structs.Pagination
                    {
                        More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                    },
                    Results = results
                };
            }
            catch (Exception ex)
            {
                return new Select2Structs.ResponseParameters
                {
                    Pagination = new Select2Structs.Pagination
                    {
                        More = false
                    },
                    Results = new List<Select2Structs.Result>(),
                };
            }
        }
        public static DataTablesStructs.ReturnedData<T> ConvertirTablaPaginada<T>(this List<T> lista, DataTablesStructs.SentParameters parameters, Func<T, dynamic> ordernarPor = null)
        {
            try
            {
                var data = lista
                    //.Skip(parameters.PagingFirstRecord)
                    //.Take(parameters.RecordsPerDraw)
                    .ToList();

                var primero = data.FirstOrDefault();

                int recordsFiltered = 0;
                if (primero != null)
                {
                    var propiedad = primero.GetType().GetProperty("TOTALROWS");
                    recordsFiltered = (int)propiedad.GetValue(primero);
                }
                else
                {
                    recordsFiltered = data.Count();
                }

                var recordsTotal = data.Count();

                return new DataTablesStructs.ReturnedData<T>
                {
                    Data = data,
                    DrawCounter = parameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal
                };
            }
            catch (Exception ex)
            {
                return new DataTablesStructs.ReturnedData<T>
                {
                    Error = ex.Message,
                    Data = new List<T>(),
                    DrawCounter = 1,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }
        }
    }
}
