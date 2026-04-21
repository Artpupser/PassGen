using PassGen.Graphics;
using PassGen.Password.Configs;

using PupaLib.Core;

namespace PassGen.Password.Builders;

public abstract class PasswordGeneratorConfigBuilder : IGraphicsInstruction {
   protected PasswordGeneratorConfig Instance { get; } = new();
   public abstract PasswordGeneratorConfig Build();
   public abstract Task<Option> Render(IGraphics graphics, CancellationToken cancellationToken);
}