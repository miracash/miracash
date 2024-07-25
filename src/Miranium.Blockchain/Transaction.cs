using System.Security.Cryptography;
using System.Text;

namespace Miranium.Blockchain;

public class Transaction
{
    public string FromAdress { get; set; }
    public string ToAdress { get; set; }
    public decimal Value { get; set; }
    public string Hash { get; set; }

    public Transaction(string from, string to, int value)
    {
        FromAdress = from;
        ToAdress = to;
        Value = value;
        Hash = CalculateHash();
    }
    
    private string CalculateHash()
    {
        string raw = $"{FromAdress}{ToAdress}{Value}";

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(raw);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    public override string ToString()
    {
        return $"Transaction : sender : {FromAdress} -> {Value} to : {ToAdress}";
    }
}