namespace TizianaTerenzi.Common.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class EventMessageLog : BaseModel<int>
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        private string serializedData;

        public EventMessageLog(object data)
            => this.Data = data;

        private EventMessageLog()
        {
        }

        public Type Type { get; private set; }

        public bool Published { get; private set; }

        public void MarkAsPublished() => this.Published = true;

        [NotMapped]
        public override DateTime? ModifiedOn { get => base.ModifiedOn; set => base.ModifiedOn = value; }

        [NotMapped]
        public object Data
        {
            get => JsonSerializer.Deserialize(this.serializedData, this.Type, this.jsonSerializerOptions);
            set
            {
                this.Type = value.GetType();

                this.serializedData = JsonSerializer.Serialize(value, this.jsonSerializerOptions);
            }
        }
    }
}
