using Crab.Resources;
using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class ResourceButtonContainer : HFlowContainer
{
    [Signal] public delegate void ButtonPressedEventHandler(DisplayResource resource);
    [Signal] public delegate void ButtonMousedOverEventHandler(DisplayResource resource);
    [Signal] public delegate void ButtonMousedAwayEventHandler();
    [Signal] public delegate void ButtonRightClickedEventHandler(DisplayResource resource);

    public override void _Ready()
    {
        base._Ready();
        ChildEnteredTree += OnResourceButtonAdded;
        ChildExitingTree += OnResourceButtonRemoved;
    }

    public void Populate(Godot.Collections.Array<DisplayResource> resources, int iconSize = 64)
    {
        Clear();
        resources.ToList()
            .ForEach(resource => AddResource(resource, iconSize));
    }

    public void Clear()
    {
        GetChildren().OfType<ResourceButton>()
            .ToList()
            .ForEach(button => RemoveResource(button.Resource));
    }

    public void AddResource(DisplayResource resource, int iconSize = 64)
    {
        var button = new ResourceButton
        {
            Resource = resource,
        };
        button.SetIconSize(iconSize);
        AddChild(button);
    }

    public void RemoveResource(DisplayResource resource)
    {
        var node = GetChildren()
            .Where(child => child is ResourceButton).Cast<ResourceButton>()
            .Where(button => button.Resource == resource).First();
        RemoveChild(node);
        node.QueueFree();
    }

    private void OnButtonPressed(DisplayResource resource) =>
        EmitSignal(SignalName.ButtonPressed, resource);
    private void OnButtonMousedOver(DisplayResource resource) =>
        EmitSignal(SignalName.ButtonMousedOver, resource);
    private void OnButtonMousedAway() =>
        EmitSignal(SignalName.ButtonMousedAway);

    private void OnButtonRightClicked(DisplayResource resource) =>
        EmitSignal(SignalName.ButtonRightClicked, resource);

    private void OnResourceButtonAdded(Node node)
    {
        if (node is not ResourceButton button) return;
        button.ButtonPressed += OnButtonPressed;
        button.ButtonMousedOver += OnButtonMousedOver;
        button.ButtonMousedAway += OnButtonMousedAway;
        button.ButtonRightClicked += OnButtonRightClicked;
    }

    private void OnResourceButtonRemoved(Node node)
    {
        if (node is not ResourceButton button || !IsInstanceValid(button)) return;
        button.ButtonPressed -= OnButtonPressed;
        button.ButtonMousedOver -= OnButtonMousedOver;
        button.ButtonMousedAway -= OnButtonMousedAway;
        button.ButtonRightClicked -= OnButtonRightClicked;
    }

    public DisplayResource GetResourceAtIndex(int index) => GetChild<ResourceButton>(index).Resource;
}
