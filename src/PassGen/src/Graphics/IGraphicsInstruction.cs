using PupaLib.Core;

namespace PassGen.Graphics;

public interface IGraphicsInstruction {
   public Task<Option> Render(IGraphics graphics, CancellationToken cancellationToken);
}