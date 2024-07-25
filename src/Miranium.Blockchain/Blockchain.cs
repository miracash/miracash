namespace Miranium.Blockchain;

public class Blockchain
{
    public List<Block> Blocks { get; set; }
    private List<Transaction> transactionPool { get; set; }
    private int _difficulty;

    public Blockchain(string genAdrr)
    {
        Blocks = new List<Block>() { CreateGenesisBlock(genAdrr) };
        transactionPool = new List<Transaction>();
        _difficulty = 4;
    }
    private Block CreateGenesisBlock(string genAdrr)
    {
        return new Block("0", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), new List<Transaction> { new Transaction("System", genAdrr, 200) });
    }
    public bool AddTransaction(Transaction transaction, string publicKey, string data, string signedData, string adress)
    {
        if (!CheckTransactionIntegrity(transaction, publicKey, data, signedData, adress))
        {
            return false;
        }
        transactionPool.Add(transaction);
        return true;
    }
    private bool CheckBlocksIntegrity()
    {
        Block currentBlock;
        Block previousBlock;
        
        for (int i = 1; i < Blocks.Count; i++)
        {
            currentBlock = Blocks[i];
            previousBlock = Blocks[i - 1];

            if (currentBlock.Header.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.Header.PreviousHash != previousBlock.Header.Hash)
            {
                return false;
            }
        }
        return true;
    }
    private bool CheckTransactionIntegrity(Transaction transaction, string publicKey, string data, string signedData, string adress)
    {
        if (Wallet.Wallet.VerifySignedData(data, signedData, publicKey) 
            && Wallet.Wallet.CheckAdressValidityWithPublickKey(publicKey, transaction.FromAdress))
        {
            decimal balance = GetBalance(transaction.FromAdress);
            return balance >= transaction.Value;
        }
        return false;
    }
    public void DisplayBlockchain()
    {
        foreach (var block in Blocks)
        {
            Console.WriteLine(block);
        }
    }
    public void MineBlock()
    {
        var newBlock = new Block(GetLatestBlock().Header.Hash, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), new List<Transaction>(transactionPool));
        transactionPool.Clear();
        newBlock.Header.Hash = MineBlock(newBlock, _difficulty);
        Blocks.Add(newBlock);
    }
    private string MineBlock(Block block, int difficulty)
    {
        string leadingZeros = new string('0', difficulty);
        string hash;
        do
        {
            block.Header.Nonce++;
            hash = block.CalculateHash();
        } while (!hash.StartsWith(leadingZeros));
        return hash;
    }
    public decimal GetBalance(string adress)
    {
        decimal balance = 0;
        foreach (var block in Blocks)
        {
            foreach (var transaction in block.Transactions)
            {
                if (adress == transaction.FromAdress)
                {
                    balance -= transaction.Value;
                }
                if (adress == transaction.ToAdress)
                {
                    balance += transaction.Value;
                }
            }
        }
        return balance;
    }
    private Block GetLatestBlock()
    {
        return Blocks.Last();
    }
}