using System.Text;

using PassGen.Crypto;
using PassGen.Password.Builders;
using PassGen.Password.Configs;
using PassGen.Password.Results;

using PupaLib.Core;

namespace PassGen.Password.Models;

[PasswordGeneratorInfo("argon", "Very secure generator password [latest]")]
public class PasswordArgonGenerator : PasswordGenerator {
   public override async Task<Option<PasswordResult>> Generate(PasswordGeneratorConfig config) {
      var lengthOption = config.Get("length").Map(x => (int)x);
      var keyOption = config.Get("key").Map(x => x.ToString() ?? string.Empty);
      var timeOption = config.Get("time").Map(x => (DateTime)x);
      if (!lengthOption.Out(out var length) || !keyOption.Out(out var key) || !timeOption.Out(out var time))
         return Option<PasswordResult>.Fail();

      var rnd = new Random(time.Second + time.Minute + time.Hour + time.Day + time.Month + time.Year);
      var password = await CryptoUtils.GenerateArgonAsync(key, length,
         Encoding.UTF8.GetBytes($"{time}{rnd.Next(int.MinValue, int.MaxValue)}{length}"));

      var result = new PasswordArgonResult {
         CreatedAt = time,
         Password = password[..length],
         KeyCode = key
      };


      return Option<PasswordResult>.Ok(result);
   }

   public override PasswordGeneratorConfigBuilder GetBuilder() {
      return new PasswordGeneratorArgonConfigBuilder();
   }
}