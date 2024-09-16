using Crab.Resources;
using Godot;

namespace Crab.Nodes;

[Tool]
public partial class GameSprite : Sprite2D
{
    private SpriteResource _resource { get; set; }

    [Export]
    public SpriteResource Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            if (value is not null)
            {
                if (value.Texture is not null) Texture = value.Texture;
            }
            else
            {
                Texture = null;
            }
        }
    }


    

}
