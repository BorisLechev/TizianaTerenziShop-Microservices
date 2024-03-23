namespace TizianaTerenzi.WebClient.ViewModels.Contacts
{
    using System;

    public class ContactMessagesViewModel
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
