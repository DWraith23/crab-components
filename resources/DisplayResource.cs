using Godot;

namespace Crab.Resources;

[GlobalClass, Tool]
/// <summary>
/// A resource that contains a display name, description, and icon.
/// </summary>
public partial class DisplayResource : Resource
{
    private string _displayName { get; set; } = string.Empty;
    private string _description { get; set; } = string.Empty;
    private Texture2D _icon { get; set; }

    [Export]
    public string DisplayName
    {
        get => _displayName;
        set
        {
            _displayName = value;
            EmitChanged();
        }
    }

    [Export]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            EmitChanged();
        }
    }

    [Export]
    public Texture2D Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            EmitChanged();
        }
    }
}