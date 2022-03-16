namespace AMS.Controllers.Models
{
    public class PaginationViewModel
    {
        public string Id { get; init; }
        public int CurrentPage { get; init; }
        public double MaxPage { get; init; }
        public int ItemsCount { get; init; }
        public string ControllerName { get; init; }
    }
}
