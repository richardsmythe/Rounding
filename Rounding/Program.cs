internal class Program
{
    //proportional rounding/allocation
    private static void Main(string[] args)
    {
        ProportionalAllocation pa = new ProportionalAllocation([2, 1, 1], 10);
        var share = pa.GetProportionalShares();
        Console.WriteLine(share);
    }

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
            double[] proportionalShare = new double[_weights.Length];
            int[] roundedShares = new int[_weights.Length];
            for (int i = 0; i < _weights.Length; i++)
            {
                proportionalShare[i] = _n * (double)_weights[i] / SumOfWeights;
                roundedShares[i] = (int)Math.Round(proportionalShare[i], MidpointRounding.AwayFromZero);
            }

            // check for discrepancies.
            var d = _n - roundedShares.Sum();
            double[] roundingErrors = new double[_weights.Length]; // the difference between the original proportional share and the rounded value
            if (d != 0)
            {
                // get the rounding error 
                for (int i = 0; i < _weights.Length; i++)
                {
                    roundingErrors[i] = proportionalShare[i] - roundedShares[i];
                }

                // adjust with smallest rounding error
                int minErrorIndex = Array.IndexOf(roundingErrors, roundingErrors.Min());
                if (d > 0)
                {
                    roundedShares[minErrorIndex] += 1;
                }
                else
                {
                    roundedShares[minErrorIndex] -= 1;
                }

                // update discrepency
                d =_n - roundedShares.Sum();
                // use -1 to mark this index has been adjusted already 
                roundingErrors[minErrorIndex] = -1;
            }
            return roundedShares;
        }



    }


}