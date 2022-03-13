namespace AMS.Web.Views.Common
{
    public static class Pagination
    {
        public static (int, double, bool) Setup(int currentPage, int totalItems, int itemsPerPage, int itemsCount)
        {
            var previousPage = currentPage - 1;

            double maxPageShow = 3;

            var maxPage = Math.Ceiling((double)totalItems / itemsPerPage);

            var shouldButtonBeDisabled = currentPage == maxPage ||
                                     itemsCount == 0;

            if (previousPage < 1)
            {
                previousPage = 1;
            }

            if (maxPage < maxPageShow)
            {
                maxPageShow = maxPage;
            }

            return (previousPage, maxPageShow, shouldButtonBeDisabled);
        }
    }
}
