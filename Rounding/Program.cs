using System;
using System.Linq;

internal class Program
{ 
    private static void Main(string[] args)
    {
        ProportionalAllocation pa = new ProportionalAllocation(new int[] { 2, 1, 1 }, 10);
        var shares = pa.GetProportionalShares();
        Console.WriteLine("Proportional Shares: " + string.Join(", ", shares));
    }

    /// <summary>
    /// Proportional allocation of n based on given weights. discrepency adjustments based on absolute,relative & weighted errors
    /// </summary>
    public class ProportionalAllocation
    {
        private int _n;
        private int[] _weights;
        public int SumOfWeights => _weights.Sum();

        public ProportionalAllocation(int[] weights, int n)
        {
            _weights = weights;
            _n = n;
        }

        public int[] GetProportionalShares()
        {
            if (_weights == null || _weights.Length == 0 || _n == 0) return new int[] { 0 };

            double[] idealProportionalShare = new double[_weights.Length];
            int[] roundedShares = new int[_weights.Length];

            for (int i = 0; i < _weights.Length; i++)
            {
                idealProportionalShare[i] = _n * (double)_weights[i] / SumOfWeights;
                roundedShares[i] = (int)Math.Round(idealProportionalShare[i], MidpointRounding.AwayFromZero);
            }

            var d = _n - roundedShares.Sum();

            d = AdjustDependency(idealProportionalShare, roundedShares, d);

            return roundedShares;
        }

        private int AdjustDependency(double[] idealProportionalShare, int[] roundedShares, int d)
        {
            while (d != 0)
            {
                double[] absoluteErrors = new double[_weights.Length];
                double[] relativeErrors = new double[_weights.Length];

                for(int i = 0; i < _weights.Length; i++)
                {
                    absoluteErrors[i] = idealProportionalShare[i]-roundedShares[i];
                    relativeErrors[i] = absoluteErrors[i] / idealProportionalShare[i];
                }

                double[] priorityScores = new double[_weights.Length];
                for (int i = 0; i < _weights.Length; i++)
                {
                    priorityScores[i] = Math.Abs(absoluteErrors[i]) + Math.Abs(relativeErrors[i]);
                }
                int highestPriorityScoreIndex = Array.IndexOf(priorityScores, priorityScores.Max());
                if (d > 0)
                {
                    roundedShares[highestPriorityScoreIndex] += 1;
                }
                else
                {
                    roundedShares[highestPriorityScoreIndex] -= 1;
                }               
                d = _n - roundedShares.Sum();
            }

            return d;
        }
    }
}
