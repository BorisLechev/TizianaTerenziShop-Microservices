namespace TizianaTerenzi.Common.Web.WebModels
{
    public interface IBasePagingViewModel
    {
        int CurrentPage { get; set; }

        int PagesCount { get; }

        int ItemsCount { get; set; }

        int ItemsPerPage { get; set; }

        int PreviousPage { get; }

        int NextPage { get; }
    }
}
