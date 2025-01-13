internal partial class Program
{ 
    private static void Main(string[] args)
    {
        ProportionalAllocation pa = new ProportionalAllocation([40, 35.5, 100, 55.6, 20], 98);

        var shares = pa.GetProportionalShares();
        Console.WriteLine("Proportional Shares: " + string.Join(", ", shares));
    }
}
