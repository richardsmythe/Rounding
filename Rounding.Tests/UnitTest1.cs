
namespace Rounding.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Negative_Distribution ()
        {
            double[] weights = [-406.0, -348.0, -246.0, 0.0];
            int n = -100;
            var pa = new ProportionalAllocation(weights,n);
            var s= pa.GetProportionalShares();
            Assert.Equal(n, s.Sum());
            Assert.Equal(s, [-41, -35, -24, 0]);
        }
    }
}