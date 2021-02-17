namespace TizianaTerenzi.Services.Tests
{
    using Xunit;

    public class SlugGeneratorTests
    {
        [Fact]
        public void SlugGeneratorShouldWorkCorrectly()
        {
            var id = 17;
            var productName = "Atlantide";
            var slugGenerator = new SlugGenerator();

            var result = slugGenerator.GenerateUrl(id, productName);

            Assert.Equal("/product/17/atlantide", result);
        }
    }
}
