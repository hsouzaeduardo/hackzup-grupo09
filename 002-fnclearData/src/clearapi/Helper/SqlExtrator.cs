using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class Extensions
{
    public static string ExtractSqlFromResponse(this string response)
    {
        // Definir um padrão para capturar o bloco SQL
        var sqlPattern = @"```sql([\s\S]*?)```";

        // Usar Regex para encontrar o bloco de código SQL
        var match = Regex.Match(response, sqlPattern);

        if (match.Success)
        {
            // Retornar o SQL encontrado, removendo os delimitadores ```sql
            return match.Groups[1].Value.Trim();
        }

        return string.Empty; // Retornar vazio se nenhum SQL for encontrado
    }

    public static string Encrypt(this string plainText, string key, string IV)
    {
        using (Aes aesAlg = Aes.Create())
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Resize(ref keyBytes, aesAlg.KeySize / 8); // Redimensiona a chave para o tamanho correto
            aesAlg.Key = keyBytes;

            var ivBytes = Encoding.UTF8.GetBytes(IV);
            Array.Resize(ref ivBytes, aesAlg.BlockSize / 8); // Redimensiona o IV para o tamanho correto
            aesAlg.IV = ivBytes;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }


}

