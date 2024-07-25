using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace Miranium.Wallet;

public class Wallet
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public string Adress { get; set; }

    public Wallet()
    {
        var key = GenerateKeyPair();
        PrivateKey = key.PrivateKey;
        PublicKey = key.PublicKey;
        Adress = GenerateAddressFromPublicKey(key.PublicKey);
    }
    
    private (string PublicKey, string PrivateKey) GenerateKeyPair()
    {
        using (var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            byte[] publicKeyBytes = ecdsa.ExportSubjectPublicKeyInfo();
            byte[] privateKeyBytes = ecdsa.ExportECPrivateKey();
            return (Convert.ToBase64String(publicKeyBytes), Convert.ToBase64String(privateKeyBytes));
        }
    }
    public static string GenerateAddressFromPublicKey(string publicKeyBase64)
    {
        byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
        byte[] sha256Hash;
        using (SHA256 sha256 = SHA256.Create())
        {
            sha256Hash = sha256.ComputeHash(publicKeyBytes);
            return BitConverter.ToString(sha256Hash).Replace("-", "").ToLower();
        }
    }
    public string SignTransaction(string data)
    {
        using (var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            ecdsa.ImportECPrivateKey(Convert.FromBase64String(PrivateKey), out _);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = ecdsa.SignData(dataBytes, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(signatureBytes);
        }
    }
    public bool VerySignedDataInternal(string data, string signedData)
    {
        using (var ecsda = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            ecsda.ImportSubjectPublicKeyInfo(Convert.FromBase64String(PublicKey), out _);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signedDataBytes = Encoding.UTF8.GetBytes(signedData);
            return ecsda.VerifyData(dataBytes, signedDataBytes, HashAlgorithmName.SHA256);
        }
    }
    public static bool VerifySignedData(string data, string signature, string publicKeyBase64)
    {
        using (var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKeyBase64), out _);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);
            return ecdsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256);
        }
    }

    public static bool CheckAdressValidityWithPublickKey(string publicKey, string adress)
    {
        if (GenerateAddressFromPublicKey(publicKey) == adress)
        {
            return true;
        }
        return false;
    }
}