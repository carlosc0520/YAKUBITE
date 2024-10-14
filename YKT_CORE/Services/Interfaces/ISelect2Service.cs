using static YKT.CORE.Structs.Select2Structs;

namespace YKT.CORE.Services.Interfaces
{
    public interface ISelect2Service
    {
        int GetCurrentPage();
        string GetQuery();
        string GetRequestType();
        string GetSearchTerm();
        RequestParameters GetRequestParameters();
    }
}
