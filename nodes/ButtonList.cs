using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class ButtonList : VBoxContainer
{
    [Signal] public delegate void ButtonPressedEventHandler(int index);

    private Godot.Collections.Array<string> _names { get; set; }

    [Export]
    private Godot.Collections.Array<string> Names
    {
        get => _names;
        set
        {
            _names = value;
            UpdateButtons();
        }
    }

    public System.Collections.Generic.List<Button> Buttons =>
        GetChildren()
            .Where(child => child is Button).Cast<Button>()
            .ToList();

    private void UpdateButtons()
    {
        Buttons.ForEach(RemoveButton);
        Names.ToList()
            .ForEach(AddButton);
    }

    private void AddButton(string name)
    {
        var button = new Button()
        {
            Text = name,
        };
        int count = GetChildCount(true);
        button.Pressed += () => OnButtonPressed(count);
        AddChild(button);
    }

    private void RemoveButton(Button button)
    {
        button.Pressed -= () => OnButtonPressed(GetIndex(true));
        RemoveChild(button);
        button.QueueFree();
    }

    private void OnButtonPressed(int index) =>
        EmitSignal(SignalName.ButtonPressed, index);
}