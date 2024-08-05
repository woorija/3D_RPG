using System.Security.Cryptography;
using System.Text;

public static class EncryptionUtility
{
    static byte[] key = Encoding.UTF8.GetBytes("I5G24jqw334x2o3q");
    static byte[] iv = Encoding.UTF8.GetBytes("L2Umv2q2UmQNlsei");

    public static string Encrypt(string _text)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            byte[] cipherBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(_text), 0, _text.Length);

            return System.Convert.ToBase64String(cipherBytes);
        }
    }

    public static string Decrypt(string _text)
    {
        byte[] cipherBytes = System.Convert.FromBase64String(_text);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
