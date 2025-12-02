using System.Security.Cryptography;
using System.Text;

namespace api.Models.Configuration.Service
{
    public class Encryption
    {
        /** encryption key configuration */
        private string? Key { get; set; }

        /** web application encryption configuration */
        public Encryption(WebApplicationBuilder inWebBuilder)
        {
            /** reading encryption key */
            this.Key = inWebBuilder.Configuration.GetValue<string>("Encryption:key");
        }

        /** get value of encryption */
        public string GetValue(string? inValue, ILogger inLogger)
        {
            if (string.IsNullOrEmpty(this.Key))
            {
                inLogger.LogError("Encryption key is empty");
                return string.Empty;
            }

            inValue = (inValue ?? "").Trim();
            if (string.IsNullOrEmpty(inValue))
            {
                inLogger.LogError("No value for encryption");
                return string.Empty;
            }

            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this.Key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(inValue.Trim());
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
    }
}
