namespace MiraCash.Blockchain;

public class BlockHeader
{
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
    public string MerkleRoot { get; set; }
    public string TimeStamp { get; set; }
    public int Nonce { get; set; }
}