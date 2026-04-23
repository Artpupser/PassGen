using System.Text;

using Konscious.Security.Cryptography;

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

      var rnd = new Random(time.Nanosecond + time.Microsecond + time.Millisecond + time.Second);
      var argon = new Argon2id(Encoding.UTF8.GetBytes(key)) {
         Salt = Encoding.UTF8.GetBytes($"{time.ToLongDateString()}{rnd.Next(int.MinValue, int.MaxValue)}{length}"),
         DegreeOfParallelism = 4,
         Iterations = 4,
         MemorySize = 1024 * 64
      };

      var bytes=  await argon.GetBytesAsync(length);
      var password = Convert.ToBase64String(bytes);
      
      var result = new PasswordArgonResult() {
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