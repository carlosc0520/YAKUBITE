using static YKT.CORE.Structs.DataTablesStructs;

namespace YKT.CORE.Services.Interfaces
{
    public interface IDataTableService
    {
        int GetDrawCounter();
        string GetOrderColumn();
        string GetOrderDirection();
        int GetPagingFirstRecord();
        int GetRecordsPerDraw();
        string GetSearchValue();
        SentParameters GetSentParameters();
        object GetPaginationObject<T>(int recordsFiltered, IEnumerable<T> data);
    }
}
