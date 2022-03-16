namespace AMS.Controllers.Models
{
    public class PaginationViewModel
    {
        public string Id { get; init; }
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int ItemsPerPage { get; init; }
        public int ItemsCount { get; init; }
        public string ControllerName { get; init; }
    }
}
