using MiraCash.Blockchain;

public class Program
{
    public static void Main()
    {
        Blockchain blockchain = new Blockchain();
        
        blockchain.AddTransaction(new Transaction("lead", "Bob", 50));
        blockchain.AddTransaction(new Transaction("Bob", "Charlie", 30));
        
        blockchain.MineBlock();

        Console.WriteLine("Balance of lead: " + blockchain.GetBalance("lead"));
        Console.WriteLine("Balance of Bob: " + blockchain.GetBalance("Bob"));
        Console.WriteLine("Balance of Charlie: " + blockchain.GetBalance("Charlie"));

        var newTransaction = new Transaction("lead", "Charlie", 20);

        if (blockchain.AddTransaction(newTransaction))
        {
            blockchain.MineBlock();
        }
        else
        {
            Console.WriteLine("Transaction is invalid due to insufficient funds.");
        }

        blockchain.DisplayBlockchain();
    }
}