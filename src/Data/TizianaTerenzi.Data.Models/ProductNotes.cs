namespace TizianaTerenzi.Data.Models
{
    using TizianaTerenzi.Data.Common.Models;

    public class ProductNotes : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int NoteId { get; set; }

        public virtual Note Note { get; set; }
    }
}
