namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;

    public class PersonalDataChatUserGroupViewModel : IMapFrom<ChatUserGroup>
    {
        public DateTime ChatGroupCreatedOn { get; set; }
    }
}
