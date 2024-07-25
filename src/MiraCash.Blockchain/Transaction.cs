namespace MiraCash.Blockchain;

public class Transaction
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Value { get; set; }

    public Transaction(string from, string to, int value)
    {
        From = from;
        To = to;
        Value = value;
    }
}