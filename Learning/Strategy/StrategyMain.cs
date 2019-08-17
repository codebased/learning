namespace Learning.Strategy
{
    /*
     * Usage examples: The Strategy pattern is very common in C# code. 
     * It’s often used in various frameworks to provide users a way to change the behavior of a class without extending it.
     * Identification: Strategy pattern can be recognized by a method that lets nested object do the actual work, 
     * as well as the setter that allows replacing that object with a different one.
     */
    public class StrategyMain
    {
        public IEncryptionStrategy EncryptionStrategy { private get; set; }

        public StrategyMain(IEncryptionStrategy encryptionStrategy)
        {
            EncryptionStrategy = encryptionStrategy;
        }

        public string StoreSecureData(string data)
        {
            return EncryptionStrategy.Encrypt(data);
        }
    }

    public class AesEncryption : IEncryptionStrategy
    {
        public string Decrypt(string encryptedData)
        {
            return $"{this.ToString()} Decrypted";
        }

        public string Encrypt(string data)
        {
            return $"{this.ToString()} Encrypted";
        }
    }
    public class DesEncryption : IEncryptionStrategy
    {
        public string Decrypt(string encryptedData)
        {
            return $"{this.ToString()} Decrypted";
        }

        public string Encrypt(string data)
        {
            return $"{this.ToString()} Encrypted";
        }
    }
}
