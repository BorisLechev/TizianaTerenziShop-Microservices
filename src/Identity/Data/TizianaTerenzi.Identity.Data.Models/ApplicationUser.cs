namespace TizianaTerenzi.Identity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using TizianaTerenzi.Common.Data.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            //this.Orders = new HashSet<Order>();
            //this.Comments = new HashSet<Comment>();
            //this.FavoriteProducts = new HashSet<FavoriteProduct>();
            //this.ProductVotes = new HashSet<ProductVote>();
            //this.ChatUserGroups = new HashSet<ChatUserGroup>();
            //this.ChatMessages = new HashSet<ChatMessage>();
            //this.UserNotifications = new HashSet<ApplicationUserNotification>();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string LastName { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsBlocked { get; set; }

        [MaxLength(200)]
        public string? ReasonToBeBlocked { get; set; }

        public string? Town { get; set; }

        public string? PostalCode { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public string? Address { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }

        //public virtual ICollection<Comment> Comments { get; set; }

        //public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }

        //public virtual ICollection<ProductVote> ProductVotes { get; set; }

        //public virtual ICollection<ChatUserGroup> ChatUserGroups { get; set; }

        //public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        //public virtual ICollection<ApplicationUserNotification> UserNotifications { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
