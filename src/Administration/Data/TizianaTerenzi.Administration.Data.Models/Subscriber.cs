namespace TizianaTerenzi.Administration.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class Subscriber : BaseDeletableModel<int>
    {
        public string Email { get; set; }
    }
}
