using System;
using System.Linq;

internal partial class Program
{ 
    private static void Main(string[] args)
    {
        ProportionalAllocation pa = new ProportionalAllocation([-406.0, -348.0, -246.0, 0.0], -100);
        var shares = pa.GetProportionalShares();
        Console.WriteLine("Proportional Shares: " + string.Join(", ", shares));
    }
}
