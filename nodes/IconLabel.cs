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
    private DisplayResource _resource { get; set; }

    [Export]
    public Texture2D Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            if (LabelIcon is not null)
                LabelIcon.Texture = value;
            if (LabelIcon is not null)
                LabelIcon.Visible = value is not null;
            if (Resource is not null && Resource.Icon != value)
                Resource.Icon = value;
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
            if (Resource is not null && Resource.DisplayName != value)
                Resource.DisplayName = value;
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

    [Export]
    public DisplayResource Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            if (value is not null)
            {
                _resource.Changed += () => ApplyResource(value);
                ApplyResource(value);
                LabelIcon.TooltipText = value.Description;
            }
            else
            {
                SetText(string.Empty);
                SetIcon(null);
            }
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
        if (!IsInstanceValid(this)) return;
        if (resource is null)
        {
            SetText(string.Empty);
            SetIcon(null);
            return;
        }
        SetText(resource.DisplayName);
        SetIcon(resource.Icon);
    }
}
