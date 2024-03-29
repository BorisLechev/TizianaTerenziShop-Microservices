namespace TizianaTerenzi.Common.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TizianaTerenzi.Common.Data.Models;

    public class EventMessageLogEntityConfiguration : IEntityTypeConfiguration<EventMessageLog>
    {
        public void Configure(EntityTypeBuilder<EventMessageLog> builder)
        {
            builder
                .HasKey(e => e.Id);

            // Use the field in EventMessage.cs Equal names for .Property() and .HasField() is required.
            builder
                .Property<string>("serializedData")
                .IsRequired()
                .HasField("serializedData");

            // Save in db the full name of Type + the namespace and assembly (AssemblyQualifiedName)
            // When get the element from db convert to Type property
            builder
                .Property(e => e.Type)
                .IsRequired()
                .HasConversion(
                    t => t.AssemblyQualifiedName,
                    t => Type.GetType(t));
        }
    }
}
