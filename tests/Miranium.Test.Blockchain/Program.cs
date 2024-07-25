using System.Diagnostics.CodeAnalysis;
using Miranium.Blockchain;
using Miranium.Wallet;

public class Program
{
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String; size: 6686MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String; size: 6686MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String; size: 6686MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String")]
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void Main()
    {
        Wallet leadWallet = new Wallet();
        Blockchain blockchain = new Blockchain(leadWallet.Adress);

        Wallet bobWallet = new Wallet();
        Wallet aliceWallet = new Wallet();
        
        var fistTransaction = new Transaction(leadWallet.Adress, bobWallet.Adress, 50);

        var t = blockchain.AddTransaction(fistTransaction, leadWallet.PublicKey, $"{fistTransaction.FromAdress}{fistTransaction.ToAdress}{fistTransaction.Value}", leadWallet.SignTransaction($"{fistTransaction.FromAdress}{fistTransaction.ToAdress}{fistTransaction.Value}"), leadWallet.Adress);
        
        blockchain.MineBlock();
        
        Console.WriteLine("Balance of lead: " + blockchain.GetBalance(leadWallet.Adress));
        Console.WriteLine("Balance of Bob: " + blockchain.GetBalance(bobWallet.Adress));
        Console.WriteLine("Balance of Charlie: " + blockchain.GetBalance(aliceWallet.Adress));
        blockchain.DisplayBlockchain();
    }
}