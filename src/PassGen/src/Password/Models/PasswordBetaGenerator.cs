using System.Text;

using PassGen.Crypto;
using PassGen.Password.Builders;
using PassGen.Password.Configs;
using PassGen.Password.Results;

using PupaLib.Core;

namespace PassGen.Password.Models;

[PasswordGeneratorInfo(name: "beta", description: "Secure generator password [oldest]")]
public sealed class PasswordBetaGenerator : PasswordGenerator {
   private const string SecretCharacters =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+{}[]|\\:;\"'<>,.?/ ";

   public override Task<Option<PasswordResult>> Generate(PasswordGeneratorConfig config) {
      var lengthOption = config.Get("length").Map(x => (int)x);
      var keyOption = config.Get("key").Map(x => x.ToString() ?? string.Empty);
      var timeOption = config.Get("time").Map(x => (DateTime)x);
      if (!lengthOption.Out(out var length) || !keyOption.Out(out var key) || !timeOption.Out(out var time)) 
         return Task.FromResult(Option<PasswordResult>.Fail());
      
      var seed = Encoding.UTF8.GetBytes(key).Sum(x => x);
      var hash = CryptoUtils.GenerateHash(length,
         $"{time.Year}{time.Month}{time.Day}{time.Hour}{time.Minute}{time.Second}{CryptoUtils.Base64(key)}");
      var rnd = new Random(seed);
      var rnd2 = new Random(seed + length);
      var password = new StringBuilder();
      password.Append(hash);
      for (var i = 0; i < password.Length; i++) {  
         var skipSymbol = rnd2.Next(int.MinValue, int.MaxValue) % 2 == rnd.Next(int.MinValue, int.MaxValue);
         if (skipSymbol) continue;
         var rndIndex = rnd.Next(0, password.Length);
         password[rndIndex] = SecretCharacters[rnd2.Next(0, SecretCharacters.Length)];
      }

      var result = new PasswordBetaResult() {
         CreatedAt = time,
         Password = password.ToString(),
         KeyCode = key,
      };

      return Task.FromResult(Option<PasswordResult>.Ok(result));
   }

   public override PasswordGeneratorConfigBuilder GetBuilder() {
      return new PasswordGeneratorBetaConfigBuilder();
   }
}