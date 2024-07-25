using System.Security.Cryptography;
using System.Text;

namespace MiraCash.Blockchain;

public class MerkleTree
{
    public static string CalculateMerkleRoot(List<Transaction> transactions)
    {
        List<string> transactionHashes = new List<string>();

        foreach (var transaction in transactions)
        {
            transactionHashes.Add(transaction.Hash);
        }
        while (transactionHashes.Count > 1)
        {
            List<string> newLevel = new List<string>();
            for (int i = 0; i < transactionHashes.Count; i += 2)
            {
                if (i + 1 < transactionHashes.Count)
                {
                    newLevel.Add(CalculateHash(transactionHashes[i] + transactionHashes[i + 1]));
                }
                else
                {
                    newLevel.Add(transactionHashes[i]);
                }
            }
            transactionHashes = newLevel;
        }
        return transactionHashes[0];
    }
    
    private static string CalculateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(sha256.ComputeHash(inputBytes));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}