using Crab.Resources;
using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class DisplayPanel : PanelContainer
{
    [Signal] public delegate void PanelPressedEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelUnpressedEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelMousedOverEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelMousedAwayEventHandler();

    private Texture2D _icon { get; set; }
    private string _text { get; set; }
    private int _iconSize { get; set; } = 24;
    private DisplayResource _resource { get; set; }
    private bool _isPressed { get; set; } = false;

    [Export]
    public DisplayResource Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            if (value is not null)
            {
                Text = value.DisplayName;
                if (value.Icon is not null) Icon = value.Icon;
            }
            else
            {
                IconLabel.Text = string.Empty;
                IconLabel.Icon = null;
            }
        }
    }

    [Export]
    public bool IsPressed
    {
        get => _isPressed;
        set
        {
            _isPressed = value;
            if (value) OnPressed();
            else OnUnpressed();
        }
    }
    [ExportGroup("IconLabel")]
    [Export]
    public Texture2D Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            if (IconLabel is not null)
                IconLabel.Icon = value;
        }
    }

    [Export]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            if (IconLabel is not null)
                IconLabel.Text = value;
        }
    }

    [Export]
    public int IconSize
    {
        get => _iconSize;
        set
        {
            _iconSize = value;
            if (IconLabel is not null)
                IconLabel.IconSize = value;
        }
    }

    [ExportGroup("Styles")]
    [Export] private StyleBox Normal { get; set; }
    [Export] private StyleBox Pressed { get; set; }
    [Export] private StyleBox Hover { get; set; }

    public IconLabel IconLabel = new();
    private PanelContainer FocusFilter = new()
    {
        FocusMode = FocusModeEnum.Click,
    };

    public override void _Ready()
    {
        base._Ready();
        GetChildren().ToList()
            .ForEach(RemoveChild);
        AddChild(IconLabel, false, InternalMode.Front);
        FocusFilter.AddThemeStyleboxOverride("panel", new StyleBoxEmpty());
        FocusFilter.FocusEntered += OnPressed;
        FocusFilter.MouseEntered += OnMousedOver;
        FocusFilter.MouseExited += OnMousedAway;
        AddChild(FocusFilter, false, InternalMode.Front);
        IsPressed = false;
    }

    public void SetStyle(string style, StyleBox box)
    {
        switch (style)
        {
            case "normal" : Normal = box; break;
            case "pressed" : Pressed = box; break;
            case "hover" : Hover = box; break;
            default : break;
        }
    }

    private void OnPanelClicked() => IsPressed = !IsPressed;

    private void OnPressed()
    {
        var style = Pressed is not null
            ? Pressed
            : Normal;
        if (style is not null) AddThemeStyleboxOverride("panel", style);
        EmitSignal(SignalName.PanelPressed, Resource);
    }

    private void OnUnpressed()
    {
        if (Normal is not null) AddThemeStyleboxOverride("panel", Normal);
        EmitSignal(SignalName.PanelUnpressed, Resource);
    }

    private void OnMousedOver()
    {
        EmitSignal(SignalName.PanelMousedOver, Resource);
        var style = Hover is not null
            ? Hover
            : Normal;
        if (!IsPressed && style is not null) AddThemeStyleboxOverride("panel", style);
    }

    private void OnMousedAway()
    {
        EmitSignal(SignalName.PanelMousedAway);
        if (!IsPressed && Normal is not null) AddThemeStyleboxOverride("panel", Normal);
    }
}
