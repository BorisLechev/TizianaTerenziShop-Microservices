namespace TizianaTerenzi.Administration.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class ContactFormEntry : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string Ip { get; set; }
    }
}
