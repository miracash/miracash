using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace MiraCash.Blockchain;

public class Block
{
    public BlockHeader Header { get; set; }
    public List<Transaction> Transactions { get; set; }

    public Block(string previousHash, string timestamp, List<Transaction> transactions, int nonce = 0)
    {
        Header = new BlockHeader();
        Header.PreviousHash = previousHash;
        Header.TimeStamp = timestamp;
        Transactions = transactions;
        Header.Nonce = nonce;
    }
    public string CalculateHash()
    {
        string raw = $"{Header.TimeStamp}{Header.MerkleRoot}{Header.PreviousHash}{Header.Nonce}";
        foreach (var transaction in Transactions)
        {
            string transactionDataRaw = $"{transaction.From}{transaction.To}{transaction.Value}";
            raw += transactionDataRaw;
        }
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputHash = Encoding.UTF8.GetBytes(raw);
            byte[] hash = sha256.ComputeHash(inputHash);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Block(PreviousHash: '{Header.PreviousHash}', Timestamp: {Header.TimeStamp}, MerkleRoot: '{Header.MerkleRoot}', Nonce: {Header.Nonce}, Hash: '{Header.Hash}')");
        sb.AppendLine("Transactions:");
        foreach (var transaction in Transactions)
        {
            sb.AppendLine(transaction.ToString());
        }
        return sb.ToString();
    }
}