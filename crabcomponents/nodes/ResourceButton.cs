using Crab.Resources;
using Godot;

namespace Crab.Nodes;

[Tool]
public partial class ResourceButton : PanelContainer
{
    [Signal] public delegate void ButtonPressedEventHandler(DisplayResource resource);
    [Signal] public delegate void ButtonMousedOverEventHandler(DisplayResource resource);
    [Signal] public delegate void ButtonMousedAwayEventHandler();

    private DisplayResource _resource { get; set; }
    private int _iconSize { get; set; } = 64;
    private Color _borderColor { get; set; }
    private Color _pressedBorderColor { get; set; }
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
                Button.TooltipText = value.DisplayName;
                if (value.Icon is not null) Button.TextureNormal = value.Icon;
            }
            else
            {
                Button.TextureNormal = null;
            }
                
        }
    }

    [Export]
    private int IconSize
    {
        get => _iconSize;
        set
        {
            _iconSize = value;
            SetIconSize(value);
        }
    }

    [Export]
    private Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            if (value != Color.FromHtml("00000000"))
            {
                var styleBox = GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
                styleBox.BorderColor = value;
                AddThemeStyleboxOverride("panel", styleBox);
            }
            else
            {
                RemoveThemeStyleboxOverride("panel");
                SetDefaultStylebox();
            }   
        }
    }

    [Export]
    private Color PressedBorderColor
    {
        get => _pressedBorderColor;
        set
        {
            _pressedBorderColor = value;
            if (IsPressed) 
            {
                var styleBox = GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
                styleBox.BorderColor = value;
                AddThemeStyleboxOverride("panel", styleBox);
            }
        }
    }

    [Export]
    private bool IsPressed
    {
        get => _isPressed;
        set
        {
            _isPressed = value;
            if (value)
            {
                var styleBox = GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
                styleBox.BorderColor = PressedBorderColor;
                AddThemeStyleboxOverride("panel", styleBox);
            }
            else
            {
                RemoveThemeStyleboxOverride("panel");
                SetDefaultStylebox();
            }
        }
    }
    
    private TextureButton Button = new()
    {
        IgnoreTextureSize = true,
        StretchMode = TextureButton.StretchModeEnum.Scale,
    };

    public override void _Ready()
    {
        base._Ready();
        
        SetDefaultStylebox();
        IconSize = _iconSize;

        Button.Pressed += OnButtonPressed;
        Button.MouseEntered += OnButtonMousedOver;
        Button.MouseExited += OnButtonMousedAway;
        AddChild(Button, false, InternalMode.Front);
    }

    public void SetIconSize(int size) => Button.CustomMinimumSize = new Vector2(size, size);
    public void SetBorderColor(Color color) => BorderColor = color;
    public void SetPressedBorderColor(Color color) => PressedBorderColor = color;

    public void ChangePressedState(bool pressed) => IsPressed = pressed;
    public void SwapPressedState() => IsPressed = !IsPressed;
    
    private void SetDefaultStylebox()
    {
        var styleBox = new StyleBoxFlat();
        if (GetThemeStylebox("panel") is StyleBoxFlat box)
        {
            styleBox = box.Duplicate(true) as StyleBoxFlat;
        } 
        styleBox.SetContentMarginAll(8f);
        styleBox.SetCornerRadiusAll(16);
        styleBox.SetBorderWidthAll(8);
        if (BorderColor != Color.FromHtml("00000000")) styleBox.BorderColor = BorderColor;
        styleBox.BorderBlend = true;
        AddThemeStyleboxOverride("panel", styleBox);
    }
    
    private void OnButtonPressed() => EmitSignal(SignalName.ButtonPressed, Resource);
    private void OnButtonMousedOver() => EmitSignal(SignalName.ButtonMousedOver, Resource);
    private void OnButtonMousedAway() => EmitSignal(SignalName.ButtonMousedAway);
}
