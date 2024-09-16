using Crab.Resources;
using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class IconLabel : HBoxContainer
{
    private Texture2D _icon { get; set; }
    private string _text { get; set; }
    private int _iconSize { get; set; } = 24;

    [Export]
    public Texture2D Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            if (LabelIcon is not null)
                LabelIcon.Texture = value;
        }
    }

    [Export]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            if (Label is not null)
                Label.Text = value;
        }
    }

    [Export]
    public int IconSize
    {
        get => _iconSize;
        set
        {
            _iconSize = value;
            if (LabelIcon is not null)
                LabelIcon.CustomMinimumSize = new Vector2(value, value);
        }
    }

    private TextureRect LabelIcon  {get; set; } = new()
        {
            CustomMinimumSize = new Vector2(24f, 24f),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
        };

    public Label Label { get; set; } = new();

    public override void _Ready()
    {
        base._Ready();
        GetChildren().ToList()
            .ForEach(RemoveChild);
        AddChild(LabelIcon, false, InternalMode.Front);
        AddChild(Label, false, InternalMode.Front);
    }

    public override void _EnterTree()
    {
        base._EnterTree();

    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public void SetText(string text) => Text = text;
    public void SetIcon(Texture2D icon) => Icon = icon;
    public void SetIconVisiblity(bool visible) => LabelIcon.Visible = visible;

    public void ApplyResource(DisplayResource resource)
    {
        SetText(resource.DisplayName);
        SetIcon(resource.Icon);
    }
}
