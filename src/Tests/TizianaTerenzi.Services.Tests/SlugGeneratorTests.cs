namespace TizianaTerenzi.Services.Tests
{
    using TizianaTerenzi.Services.SlugGenerator;
    using Xunit;

    public class SlugGeneratorTests
    {
        [Fact]
        public void SlugGeneratorShouldWorkCorrectly()
        {
            var id = 17;
            var productName = "Atlantide";
            var slugGenerator = new SlugGeneratorService();

            var result = slugGenerator.GenerateUrl(id, productName);

            Assert.Equal("/product/17/atlantide", result);
        }
    }
}
