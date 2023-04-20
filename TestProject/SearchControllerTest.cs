using Common;
using SearchAPI.Controllers;

namespace TestProject
{
    public class SearchControllerTest
    {
        [Fact]
        public async Task Test1()
        {
            //Arrange
            var controller = new SearchController();

            // Act
            SearchResult result = await controller.Search("wolves", 10);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(1, result?.Documents.Count);

        }
    }
}