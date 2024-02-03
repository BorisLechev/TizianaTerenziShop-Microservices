namespace TizianaTerenzi.Administration.Web.Models.Products
{
    public class EditProductInputModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string NoteIds { get; set; }

        public int ProductTypeId { get; set; }

        public int FragranceGroupId { get; set; }

        public int YearOfManufacture { get; set; }
    }
}
