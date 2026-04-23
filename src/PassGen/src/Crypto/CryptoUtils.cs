using System.Security.Cryptography;
using System.Text;

using Konscious.Security.Cryptography;

namespace PassGen.Crypto;

public static class CryptoUtils {
   public static string GenerateHash(int length, string key) {
      var hash = new StringBuilder();
      for (var i = 0; i < length + 1 % GenerateSha256("*").Length; i++)
         hash.Append(GenerateSha256(key));
      return hash.ToString()[..length];
   }

   public static string Base64(string str) {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
   }

   public static async Task<string> GenerateArgonAsync(string secret, int length, byte[]? salt = null) {
      salt ??= RandomNumberGenerator.GetBytes(16);
      var argon2 = new Argon2id(Encoding.UTF8.GetBytes(secret)) {
         Salt = salt,
         DegreeOfParallelism = 4,
         Iterations = 4,
         MemorySize = ushort.MaxValue
      };
      var hash = await argon2.GetBytesAsync(length);
      return Convert.ToBase64String(hash);
   }

   public static string GenerateSha512(string content) {
      var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(content));
      return Convert.ToHexString(bytes);
   }

   public static string GenerateSha256(string content) {
      var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
      return Convert.ToHexString(bytes);
   }

   public static string GenerateMd5(string content) {
      var bytes = MD5.HashData(Encoding.UTF8.GetBytes(content));
      return Convert.ToHexString(bytes);
   }
}