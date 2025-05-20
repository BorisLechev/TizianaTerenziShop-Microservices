namespace TizianaTerenzi.Common.Messages.Products
{
    using MassTransit;

    public class ProductAddedInTheCartMessage : CorrelatedBy<Guid>
    {
        public ProductAddedInTheCartMessage()
        {
            this.CorrelationId = NewId.NextGuid();
        }

        //private readonly Guid correlationId;

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal Price { get; set; }

        public Guid CorrelationId { get; init; }
        //public Guid CorrelationId
        //{
        //    get
        //    {
        //        return this.correlationId;
        //    }

        //    init
        //    {
        //        this.correlationId = NewId.NextGuid();
        //    }
        //}
    }
}
