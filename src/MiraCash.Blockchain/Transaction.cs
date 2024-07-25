using System.Security.Cryptography;
using System.Text;

namespace MiraCash.Blockchain;

public class Transaction
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Value { get; set; }
    public string Hash { get; set; }

    public Transaction(string from, string to, int value)
    {
        From = from;
        To = to;
        Value = value;
        Hash = CalculateHash();
    }
    
    private string CalculateHash()
    {
        string raw = $"{From}{To}{Value}";

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(raw);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}