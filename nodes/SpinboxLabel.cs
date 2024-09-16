using Crab.Resources;
using Godot;
using System;
using System.Linq;

namespace Crab.Nodes;

[Tool]
/// <summary>
/// An  IconLabel with a spinbox.
/// </summary>
public partial class SpinboxLabel : HBoxContainer
{
    [Signal] public delegate void ValueChangedEventHandler(float value);

    private ValueDisplayResource _resource { get; set; }
    private string _text { get; set; } = string.Empty;
    private Texture2D _icon { get; set; }
    private bool _iconShown { get; set; } = true;
    private float _value { get; set; } = 0f;
    private float _minValue { get; set; } = 0f;
    private float _maxValue { get; set; } = 100f;
    private float _step { get; set; } = 1f;
    private string _prefix { get; set; } = string.Empty;
    private string _suffix { get; set; } = string.Empty;
    private bool _round { get; set; } = false;
    private bool _canEdit { get; set; } = true;

    [Export]
    /// <summary>
    /// The resource to display.
    /// </summary>
    public ValueDisplayResource Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            if (value is not null)
            {
                Text = value.DisplayName;
                Value = value.Value;
                if (value.Icon is not null) Icon = value.Icon;
            }
            else
            {
                IconLabel.Text = string.Empty;
                IconLabel.Icon = null;
            }
        }
    }

    #region  IconLabel
    [ExportGroup("IconLabel")]
    [Export]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            IconLabel.SetText(value);
        }
    }

    [Export]
    public Texture2D Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            IconLabel.Icon = value;
        }
    }

    [Export]
    public bool IconShown
    {
        get => _iconShown;
        set
        {
            _iconShown = value;
            IconLabel.SetIconVisiblity(value);
        }
    }
    #endregion

    #region Spinbox

    [ExportGroup("Spinbox")]
    [Export]
    public float Value
    {
        get => _value;
        set
        {
            _value = SetSpinboxValue(value);
        }
    }

    [Export]
    public float MinValue
    {
        get  => _minValue;
        set
        {
            _minValue = value;
            SpinBox.MinValue = value;
        }
    }

    [Export]
    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            SpinBox.MaxValue = value;
        }
    }

    [Export]
    public float Step
    {
        get => _step;
        set
        {
            if (value == 0)
            value = 1f;
            _step = value;
            SpinBox.Step = MaxValue;
        }
    }

    [Export]
    public string Prefix
    {
        get => _prefix;
        set
        {
            _prefix = value;
            SpinBox.Prefix = value;
        }
    }

    [Export]
    public string Suffix
    {
        get => _suffix;
        set
        {
            _suffix = value;
            SpinBox.Suffix = value;
        }
    }

    [Export]
    public bool Round
    {
        get => _round;
        set
        {
            _round = value;
            SpinBox.Rounded = value;
        }
    }

    [Export]
    public bool CanEdit
    {
        get => _canEdit;
        set
        {
            _canEdit = value;
            SpinBox.Editable = value;
        }
    }

    #endregion

    public override void _Ready()
    {
        base._Ready();
        GetChildren().ToList()
            .ForEach(RemoveChild);
        IconLabel.SetIconVisiblity(IconShown);
        IconLabel.Icon = Icon;
        IconLabel.SetText(Text);
        IconLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        SpinBox.Value = Value;
        SpinBox.MinValue = MinValue;
        SpinBox.MaxValue = MaxValue;
        SpinBox.Step = Step;
        SpinBox.Prefix = Prefix;
        SpinBox.Suffix = Suffix;
        SpinBox.Rounded = Round;
        SpinBox.Editable = CanEdit;
        SpinBox.ValueChanged += OnValueChanged;
        
        SpinBox.Alignment = HorizontalAlignment.Center;
        AddChild(IconLabel, false, InternalMode.Front);
        AddChild(SpinBox, false, InternalMode.Front);
    }

    public IconLabel IconLabel {get; set; } = new();

    private SpinBox SpinBox {get; set; } = new();

    private float SetSpinboxValue(float value)
    {
        if (value == Value) return value;
        var result = Math.Clamp(value, MinValue, MaxValue);
        result -= result % Step;
        if (Round) result = MathF.Round(result, 0);
        _value = result;
        SpinBox.Value = result;
        return result;
    }

    private void OnValueChanged(double value)
    {
        Value = (float)value;
        EmitSignal(SignalName.ValueChanged, Value);
    }
}
