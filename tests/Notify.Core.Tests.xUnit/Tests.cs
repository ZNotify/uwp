using System;
using System.Linq;
using System.Threading.Tasks;

using Notify.Core.Services;

using Xunit;

namespace Notify.Core.Tests.XUnit
{
    // TODO: Add appropriate unit tests.
    public class Tests
    {
        [Fact]
        public void Test1()
        {
        }

        // Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public async Task EnsureSampleDataServiceReturnsListDetailsDataAsync()
        {
            var actual = await SampleDataService.GetListDetailsDataAsync();

            Assert.NotEmpty(actual);
        }
    }
}
