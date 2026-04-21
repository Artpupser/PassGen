using PassGen.Password.Builders;
using PassGen.Password.Configs;
using PassGen.Password.Results;

using PupaLib.Core;

namespace PassGen.Password.Models;

public abstract class PasswordGenerator {
   public abstract Task<Option<PasswordResult>> Generate(PasswordGeneratorConfig config);
   public abstract PasswordGeneratorConfigBuilder GetBuilder();
}