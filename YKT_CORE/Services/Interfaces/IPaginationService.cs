using static YKT.CORE.Structs.PaginationStructs;

namespace YKT.CORE.Services.Interfaces
{
    public interface IPaginationService
    {
        int GetRecordsPerDraw();
        int GetPage();
        string GetSearchValue();
        SentParameters GetSentParameters();
    }
}
