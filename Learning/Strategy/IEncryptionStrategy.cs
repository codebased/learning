namespace Learning.Strategy
{
    public interface IEncryptionStrategy
    {
        string Encrypt(string data);

        string Decrypt(string encryptedData);
    }
}