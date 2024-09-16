using Crab.Resources;
using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class ValueLabel : HBoxContainer
{
    private Texture2D _icon { get; set; }
    private string _text { get; set; }
    private float _value { get; set; }
    private int _iconSize { get; set; } = 24;
    private ValueDisplayResource _resource { get; set; }

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
            if (DisplayNameLabel is not null)
                DisplayNameLabel.Text = value;
            if (Resource is not null && Resource.DisplayName != value)
                Resource.DisplayName = value;
        }
    }

    [Export]
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            if (ValueDisplayLabel is not null)
                ValueDisplayLabel.Text = value.ToString();
            if (Resource is not null && Resource.Value != value)
                Resource.Value = value;
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
    public ValueDisplayResource Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            if (value is not null) 
            {
                ApplyResource(value);
                _resource.Changed += () => ApplyResource(value);
                LabelIcon.TooltipText = value.Description;
            }
            else
            {
                SetText(string.Empty);
                SetIcon(null);
                SetValue(0);
            }
        }
    }

    private TextureRect LabelIcon  {get; set; } = new()
        {
            CustomMinimumSize = new Vector2(24f, 24f),
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
        };

    public Label DisplayNameLabel { get; set; } = new();
    public Label ValueDisplayLabel { get; set; } = new();

    public override void _Ready()
    {
        base._Ready();
        GetChildren().ToList()
            .ForEach(RemoveChild);
        AddChild(LabelIcon, false, InternalMode.Front);
        AddChild(DisplayNameLabel, false, InternalMode.Front);
        AddChild(ValueDisplayLabel, false, InternalMode.Front);
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
    public void SetValue(float value) => Value = value;
    public void SetIconVisiblity(bool visible) => LabelIcon.Visible = visible;

    public void ApplyResource(ValueDisplayResource resource)
    {
        SetText(resource.DisplayName);
        SetIcon(resource.Icon);
        SetValue(resource.Value);
    }
}
