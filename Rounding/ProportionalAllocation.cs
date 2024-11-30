/// <summary>
/// Proportional allocation of n based on given weights. discrepancy adjustments based on absolute, relative & weighted errors
/// </summary>
public class ProportionalAllocation
{
    private int _n;
    private double[] _weights;
    public double SumOfWeights => _weights.Sum();

    public ProportionalAllocation(double[] weights, int n)
    {
        _weights = weights;
        _n = n;
        if (SumOfWeights >= double.MaxValue) 
            throw new OverflowException("The total weight is too large.");
        if (_weights.Length > 0 && _weights.Max() * Math.Abs((double)(_n + 1)) >= double.MaxValue)
            throw new OverflowException("Weights too large, potential scaling overflow");       

    }

    public int[] GetProportionalShares()
    {
        if (_weights == null || _weights.Length == 0 || _n == 0) return [0];  
       
        double[] idealProportionalShare = new double[_weights.Length];

        int[] roundedShares = new int[_weights.Length];
        for (int i = 0; i < _weights.Length; i++)
        {
            idealProportionalShare[i] = _n * _weights[i] / SumOfWeights;

            if (idealProportionalShare[i] > int.MaxValue || idealProportionalShare[i] < int.MinValue)
            {
                throw new OverflowException("Computed proportional shares exceed allowable range.");
            }
            roundedShares[i] = CustomRound(idealProportionalShare[i]);
        }
        
        var d = _n - roundedShares.Sum();
        if (d != 0)
        {
            d = AdjustDependency(idealProportionalShare, roundedShares, d);
        }      

        return roundedShares;
    }

    private int CustomRound(double value) => value < 0 ? (int)(value - 0.5) : (int)(value + 0.5);
    
    private int AdjustDependency(double[] idealProportionalShare, int[] roundedShares, int d)
    {
        while (d != 0)
        {
            double[] absoluteErrors = new double[_weights.Length];
            double[] relativeErrors = new double[_weights.Length];
            for (int i = 0; i < _weights.Length; i++)
            {
                absoluteErrors[i] = idealProportionalShare[i] - roundedShares[i];
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
            if (d < 0)
            {
                roundedShares[highestPriorityScoreIndex] -= 1;
            }

            d = _n - roundedShares.Sum();
        }

        return d;
    }
}
