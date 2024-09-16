using Godot;

namespace Crab.Nodes;

[Tool]
public partial class PopupScene : CenterContainer
{
    public virtual void Close() => QueueFree();
}
