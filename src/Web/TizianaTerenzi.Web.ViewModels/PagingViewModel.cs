namespace TizianaTerenzi.Web.ViewModels
{
    using System;

    public class PagingViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount => (int)Math.Ceiling(this.ItemsCount / (decimal)this.ItemsPerPage);

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.PagesCount ? this.PagesCount : this.CurrentPage + 1;
    }
}
