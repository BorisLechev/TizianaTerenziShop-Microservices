namespace TizianaTerenzi.Data.Models
{
    using TizianaTerenzi.Data.Common.Models;

    public class Subscriber : BaseDeletableModel<int>
    {
        public string Email { get; set; }
    }
}
