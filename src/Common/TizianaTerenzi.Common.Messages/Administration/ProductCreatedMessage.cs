namespace TizianaTerenzi.Common.Messages.Administration
{
    public class ProductCreatedMessage
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte[] Picture { get; set; }

        public IEnumerable<int> NoteIds { get; set; }

        public int ProductTypeId { get; set; }

        public int FragranceGroupId { get; set; }

        public int YearOfManufacture { get; set; }
    }
}
