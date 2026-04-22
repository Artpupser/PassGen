using PupaLib.Core;

namespace PassGen.UI;

public interface IGraphicsElement {
   public Task Render(Graphics graphics, CancellationToken cancellationToken = default);
}