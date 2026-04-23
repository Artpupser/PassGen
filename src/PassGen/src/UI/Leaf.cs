
namespace PassGen.UI;

public abstract class Leaf : IGraphicsElement {
   public abstract IGraphicsElement? Parent { get; }
   public abstract Task Render(Graphics graphics, CancellationToken cancellationToken = default);
}