using Godot;

namespace Crab.Resources;

[GlobalClass, Tool]
/// <summary>
/// A resource that contains an array of resources.
/// </summary>
public partial class ResourceArray : Resource
{
    private Godot.Collections.Array<Resource> _resources { get; set; } = [];

    [Export]
    /// <summary>
    /// An array of resources.
    /// </summary>
    public Godot.Collections.Array<Resource> Resources
    {
        get => _resources;
        set
        {
            _resources = value;
            EmitChanged();
        }
    }
}