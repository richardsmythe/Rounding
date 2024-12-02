
namespace Rounding.Tests
{
    public class RoundingTests
    {
        public static IEnumerable<object[]> TestCases()
        {
            yield return new object[] { new double[] { 100.0, 300.0, 600.0 }, 100, new double[] { 10, 30, 60 } };
            yield return new object[] { new double[] { 50.0, 150.0, 300.0 }, 75, new double[] { 7.5, 22.5, 45 } };
            yield return new object[] { new double[] { 200.0, 400.0, 800.0 }, 50, new double[] { 7, 14, 29 } };
        }

        [Fact]
        public void Test_Negative_Distribution ()
        {
            double[] weights = [-500.0, -300.0, -200.0, 0.0];
            int n = -100;
            var pa = new ProportionalAllocation(weights,n);
            var s= pa.GetProportionalShares();
            Assert.Equal(n, s.Sum());
            Assert.Equal(s, [-50, -30, -20, 0]);
        }

        [Fact]
        public void Test_Zero_Distribution()
        {
            double[] weights = [4.0, 644, 33.0, 0.0];
            int n = 0;
            var pa = new ProportionalAllocation(weights, n);
            var s = pa.GetProportionalShares();
            Assert.Equal(n, s.Sum());
            Assert.Equal(s, [0]);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Test_Error_Adjustment_Accuracy(double[] weights, int n, double[] expectedShares)
        {
            var pa = new ProportionalAllocation(weights, n);
            var s = pa.GetProportionalShares();
            Assert.InRange(s[0], expectedShares[0] - 1, expectedShares[0] + 1);
            Assert.InRange(s[1], expectedShares[1] - 1, expectedShares[1] + 1);
            Assert.InRange(s[2], expectedShares[2] - 1, expectedShares[2] + 1);
            Assert.Equal(n, s.Sum());
            Assert.True(Math.Abs(s[0] - expectedShares[0]) < 2, "Share 1 error too large");
            Assert.True(Math.Abs(s[1] - expectedShares[1]) < 2, "Share 2 error too large");
            Assert.True(Math.Abs(s[2] - expectedShares[2]) < 2, "Share 3 error too large");
        }


        [Fact]
        public void Test_Negative_Weights()
        {
            double[] weights = [-99.0, -100.0, -800.0, -1.0];
            int n = 100;
            var pa = new ProportionalAllocation(weights, n);
            var s = pa.GetProportionalShares();
            Assert.Equal(n, s.Sum());
            Assert.Equal(s, [10, 10, 80, 0]);
        }

        [Fact]
        public void Test_Discrepancy_Adjustments()
        {
            double[] weights = { 40, 35.5, 100, 55.6, 20 };
            int n = 98; 
            var pa = new ProportionalAllocation(weights, n);
            var s = pa.GetProportionalShares();
            Assert.Equal(n, s.Sum());
            Assert.Equal([15, 14, 39, 22, 8], s);
        }



    }
}