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

    public override void _Ready()
    {
        base._Ready();
        ChildEnteredTree += OnResourceButtonAdded;
        ChildExitingTree += OnResourceButtonRemoved;
    }

    public void Populate(Godot.Collections.Array<DisplayResource> resources, int iconSize = 64)
    {
        resources.ToList()
            .ForEach(resource => AddResource(resource, iconSize));
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

    private void OnResourceButtonAdded(Node node)
    {
        if (node is not ResourceButton button) return;
        button.ButtonPressed += OnButtonPressed;
        button.ButtonMousedOver += OnButtonMousedOver;
        button.ButtonMousedAway += OnButtonMousedAway;
    }

    private void OnResourceButtonRemoved(Node node)
    {
        if (node is not ResourceButton button) return;
        button.ButtonPressed -= OnButtonPressed;
        button.ButtonMousedOver -= OnButtonMousedOver;
        button.ButtonMousedAway -= OnButtonMousedAway;
    }
}