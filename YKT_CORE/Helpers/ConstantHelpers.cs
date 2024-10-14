namespace YKT.CORE.Helpers
{
    public static partial class ConstantHelpers
    {
        public static class SELECT2
        {
            public static class DEFAULT
            {
                public const int PAGE_SIZE = 10;
            }

            public static class SERVER_SIDE
            {
                public static class REQUEST_PARAMETERS
                {
                    public const string CURRENT_PAGE = "page";
                    public const string QUERY = "q";
                    public const string REQUEST_TYPE = "_type";
                    public const string SEARCH_TERM = "term";
                }

                public static class REQUEST_TYPE
                {
                    public const string QUERY = "query";
                    public const string QUERY_APPEND = "query_append";
                }
            }
        }
        public static class DATATABLE
        {
            public static class SERVER_SIDE
            {
                public static class DEFAULT
                {
                    public const string ORDER_DIRECTION = "ASC";
                }

                public static class SENT_PARAMETERS
                {
                    public const string DRAW_COUNTER = "draw";
                    public const string PAGING_FIRST_RECORD = "start";
                    public const string RECORDS_PER_DRAW = "length";
                    public const string SEARCH_VALUE = "search[value]";
                    public const string SEARCH_REGEX = "search[regex]";
                    public const string ORDER_COLUMN = "order[0][column]";
                    public const string ORDER_DIRECTION = "order[0][dir]";
                }
            }
        }
        public static class PAGINATION
        {
            public static class SERVER_SIDE
            {
                public static class SENT_PARAMETERS
                {
                    public const string PAGE = "page";
                    public const string RECORDS_PER_DRAW_PAGINATION = "rpdraw";
                    public const string SEARCH_VALUE_PAGINATION = "srch";
                }
            }
        }
        public static class FORMATS
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy HH:mm:ss";
        }
        public static class TIMEZONEINFO
        {
            public const bool DisableDaylightSavingTime = true;
            public const int Gmt = -5;
            public const string LINUX_TIMEZONE_ID = "America/Bogota";
            public const string OSX_TIMEZONE_ID = "America/Cayman";
            public const string WINDOWS_TIMEZONE_ID = "SA Pacific Standard Time";
        }
    }
}
