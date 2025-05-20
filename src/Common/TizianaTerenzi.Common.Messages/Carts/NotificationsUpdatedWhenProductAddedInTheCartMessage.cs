namespace TizianaTerenzi.Common.Messages.Carts
{
    using MassTransit;

    public class NotificationsUpdatedWhenProductAddedInTheCartMessage : CorrelatedBy<Guid>
    {
        public NotificationsUpdatedWhenProductAddedInTheCartMessage()
        {
            this.CorrelationId = NewId.NextGuid();
        }

        //private Guid correlationId;

        public Guid CorrelationId { get; init; }

        //public Guid CorrelationId
        //{
        //    get => this.correlationId;
        //    init => this.correlationId = NewId.NextGuid();
        //}

        public string UserId { get; set; }

        public int NumberOfProductsInTheUsersCart { get; set; }
    }
}
