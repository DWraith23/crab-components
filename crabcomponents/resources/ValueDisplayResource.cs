using System;
using Crab.Resources;
using Godot;

namespace CrabComponents.addons.crabcomponents.resources;

[GlobalClass, Tool]
public partial class ValueDisplayResource : DisplayResource
{
    private float _value { get; set; } = 0f;
    [Export]
    public float Value
    { 
        get => _value;
        set
        {
            _value = value;
            EmitChanged();
        }
    }

    public int RoundedValue => (int)Math.Round(Value, 0);
}