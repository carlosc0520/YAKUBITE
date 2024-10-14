namespace YKT.CORE.Structs
{
    public class PaginationStructs
    {
        public struct SentParameters
        {
            public int RecordsPerDraw { get; set; }
            public int Page { get; set; }
            public string SearchValue { get; set; }
            public int RecordsToTake
            {
                get
                {
                    return (Page - 1) * RecordsPerDraw;
                }
            }
        }
        public struct ReturnedData<T>
        {
            public IEnumerable<T> Data { get; set; }
            public PaginationData PaginationData { get; set; }

        }
        public struct PaginationData
        {
            public int RecordsTotal { get; set; }
            public int Active { get; set; }
            public int RecordsPerDraw { get; set; }
            public decimal TotalPages
            {
                get
                {
                    return Math.Ceiling((Convert.ToDecimal(RecordsTotal) / Convert.ToDecimal(RecordsPerDraw)));
                }
            }
        }
    }
}
