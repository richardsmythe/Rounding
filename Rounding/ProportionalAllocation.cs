/// <summary>
/// Proportional allocation of n based on given weights. discrepancy adjustments based on absolute, relative & weighted errors
/// </summary>
public class ProportionalAllocation
{
    private int _n;
    private double[] _weights;

    private readonly double _sumOfWeights;
    public ProportionalAllocation(double[] weights, int n)
    {
        _weights = weights;
        _n = n;
        _sumOfWeights = weights.Sum();

    }
    public int[] GetProportionalShares()
    {
        if (_weights == null || _weights.Length == 0 || _n == 0) return [0];  
       
        double[] idealProportionalShare = new double[_weights.Length];
        int[] roundedShares = new int[_weights.Length];

        for (int i = 0; i < _weights.Length; i++)
        {
            idealProportionalShare[i] = _n * _weights[i] / _sumOfWeights;
            if (idealProportionalShare[i] > int.MaxValue || idealProportionalShare[i] < int.MinValue)
            {
                throw new OverflowException("Computed proportional shares exceed allowable range.");
            }
            roundedShares[i] = CustomRound(idealProportionalShare[i]);
        }
        var d = _n - roundedShares.Sum();
        if (d != 0) d = AdjustDependency(idealProportionalShare, roundedShares, d);

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
                priorityScores[i] = Math.Abs(absoluteErrors[i]) / _sumOfWeights + Math.Abs(relativeErrors[i]);
            }

            int highestPriorityScoreIndex = getHighestPriorityIndex(priorityScores);
            if (d > 0) roundedShares[highestPriorityScoreIndex] += 1;
            if (d < 0) roundedShares[highestPriorityScoreIndex] -= 1;
            d = _n - roundedShares.Sum();
        }

        return d;
    }

    private int getHighestPriorityIndex(double[] priorityScores)
    {
        int highestPriority = -1;
        double maxPriorityScore = double.MinValue;
        for(int i = 0; i < priorityScores.Length;i++)
        {
            if(priorityScores[i] > maxPriorityScore)
            {
                maxPriorityScore = priorityScores[i];
                highestPriority = i;
            }
        }
        return highestPriority;
    }
}
