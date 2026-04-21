using System.Text;

using PassGen.Crypto;
using PassGen.Password.Builders;
using PassGen.Password.Configs;
using PassGen.Password.Results;

using PupaLib.Core;

namespace PassGen.Password.Models;

[PasswordGeneratorInfo(name: "alpha", description: "Simplify generator password [not recommended]")]
public sealed class PasswordAlphaGenerator : PasswordGenerator {
   public override Task<Option<PasswordResult>> Generate(PasswordGeneratorConfig config) {
      var lengthOption = config.Get("length").Map(x => (int)x);
      var keyOption = config.Get("key").Map(x => x.ToString() ?? string.Empty);
      if (!lengthOption.Out(out var length) || !keyOption.Out(out var key)) 
         return Task.FromResult(Option<PasswordResult>.Fail());
      
      var hash = new StringBuilder();
      for (var i = 0; i < length + 1 % CryptoUtils.GenerateSha256("*").Length; i++)
         hash.Append(CryptoUtils.GenerateSha256(key)); 
      var ex = hash.ToString()[..length];
      hash.Clear();
      var rnd = new Random(key.Select(x => (byte)x).Sum(x => x % 2 == 0 ? x + length : x - length));
      while (ex.Length != hash.Length)
         hash.Append(ex[rnd.Next(0, ex.Length)]);
      var result = new PasswordAlphaResult {
         Password = hash.ToString(),
         CreatedAt = DateTime.UtcNow,
         KeyCode = key,
      };
      return Task.FromResult(Option<PasswordResult>.Ok(result));
   }

   public override PasswordGeneratorConfigBuilder GetBuilder() {
      return new PasswordGeneratorAlphaConfigBuilder();
   }
}