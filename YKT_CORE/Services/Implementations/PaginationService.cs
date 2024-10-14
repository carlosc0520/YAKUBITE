using YKT.CORE.Helpers;
using YKT.CORE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using static YKT.CORE.Structs.PaginationStructs;

namespace YKT.CORE.Services.Implementations
{
    public class PaginationService : IPaginationService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public PaginationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetPage()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[ConstantHelpers.PAGINATION.SERVER_SIDE.SENT_PARAMETERS.PAGE], out int result);
            return result;
        }

        public int GetRecordsPerDraw()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[ConstantHelpers.PAGINATION.SERVER_SIDE.SENT_PARAMETERS.RECORDS_PER_DRAW_PAGINATION], out int result);
            return result;
        }

        public string GetSearchValue()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query[ConstantHelpers.PAGINATION.SERVER_SIDE.SENT_PARAMETERS.SEARCH_VALUE_PAGINATION];
        }

        public SentParameters GetSentParameters()
        {
            return new SentParameters
            {
                RecordsPerDraw = GetRecordsPerDraw(),
                Page = GetPage(),
                SearchValue = GetSearchValue(),
            };
        }
    }
}
