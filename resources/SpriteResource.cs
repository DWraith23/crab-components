using Godot;

namespace Crab.Resources;

[GlobalClass, Tool]
public partial class SpriteResource : DisplayResource
{
    private Texture2D _texture { get; set; }

    [Export]
    public Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture = value;
            EmitChanged();
        }
    }
}