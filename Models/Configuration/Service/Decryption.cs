using System.Security.Cryptography;
using System.Text;

namespace api.Models.Configuration.Service
{
    public class Decryption
    {
        /** decryption key configuration */
        private string? Key { get; set; }

        /** web application decryption configuration */
        public Decryption(WebApplicationBuilder inWebBuilder)
        {
            /** reading decryption key */
            this.Key = inWebBuilder.Configuration.GetValue<string>("Encryption:key");
        }

        /** get value of decryption */
        public string GetValue(string? inValue, ILogger inLogger)
        {
            if (string.IsNullOrEmpty(this.Key))
            {
                inLogger.LogError("Decryption key is empty");
                return string.Empty;
            }

            inValue = (inValue ?? "").Trim();
            if (string.IsNullOrEmpty(inValue))
            {
                inLogger.LogError("No value for decryption");
                return string.Empty;
            }

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(inValue);
            string getData = string.Empty;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this.Key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            getData = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return getData;
        }
    }
}
