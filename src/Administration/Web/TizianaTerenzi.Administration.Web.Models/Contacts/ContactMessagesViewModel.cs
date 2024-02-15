namespace TizianaTerenzi.Administration.Web.Models.Contacts
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Services.Mapping;

    public class ContactMessagesViewModel : IMapFrom<ContactFormEntry>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string Ip { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
