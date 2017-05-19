namespace GreentubeGameTask.Core.Entities
{
    public class FilterModel
    {
        public FilterModel()
        {
            PageSize = 10;
            CurrentPage = 1;
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public long TotalRecords { get; set; }
        public bool SortDirection { get; set; }
        public string SortParameter { get; set; }
        public string ApiCallDate { get; set; }
        public int SkipRecords => (CurrentPage - 1)*PageSize;
    }
}