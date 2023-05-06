namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataChatUserGroupViewModel : IMapFrom<ChatUserGroup>
    {
        public string ChatGroupName { get; set; }

        public DateTime ChatGroupCreatedOn { get; set; }
    }
}
