namespace TizianaTerenzi.Products.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class ProductNote : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int NoteId { get; set; }

        public virtual Note Note { get; set; }
    }
}
