using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace mspersistence.Helper
{
    public static class Extensions
    {
        public static string Decrypt(this string cipherText, string key, byte[] iv)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                Array.Resize(ref keyBytes, aesAlg.KeySize / 8); // Ajusta o tamanho da chave
                aesAlg.Key = keyBytes;

                // Se o IV foi armazenado no início do texto cifrado, extraia os primeiros 16 bytes
                if (iv == null || iv.Length != aesAlg.BlockSize / 8)
                {
                    iv = new byte[aesAlg.BlockSize / 8];
                    Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                }
                aesAlg.IV = iv; // Usa o IV passado ou extraído do texto cifrado

                // O restante são os dados criptografados (após o IV)
                var cipherBytes = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

    }
}
