namespace MelegPerfumes.Data.Models
{
    using MelegPerfumes.Data.Common.Models;

    public class Subscriber : BaseDeletableModel<int>
    {
        public string Email { get; set; }
    }
}
