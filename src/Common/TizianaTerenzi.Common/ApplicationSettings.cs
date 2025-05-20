namespace TizianaTerenzi.Common
{
    public class ApplicationSettings
    {
        public string Secret { get; private set; }

        public bool SeedInitialRegularUser { get; private set; } = true;
    }
}
