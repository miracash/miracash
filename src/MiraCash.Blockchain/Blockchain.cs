namespace MiraCash.Blockchain;

public class Blockchain
{
    public List<Block> Blocks { get; set; }
    private List<Transaction> transactionPool { get; set; }
    private int _difficulty;

    public Blockchain()
    {
        Blocks = new List<Block>() { CreateGenesisBlock() };
        transactionPool = new List<Transaction>();
        _difficulty = 7;
    }
    private Block CreateGenesisBlock()
    {
        return new Block("0", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), new List<Transaction> { new Transaction("System", "lead", 200) });
    }
    public bool AddTransaction(Transaction transaction)
    {
        if (!CheckTransactionIntegrity(transaction))
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
    private bool CheckTransactionIntegrity(Transaction transaction)
    {
        decimal balance = GetBalance(transaction.From);
        return balance >= transaction.Value;
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
                if (adress == transaction.From)
                {
                    balance -= transaction.Value;
                }
                if (adress == transaction.To)
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