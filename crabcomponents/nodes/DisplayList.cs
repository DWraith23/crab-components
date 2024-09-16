using Crab.Resources;
using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class DisplayList : VBoxContainer
{
    [Signal] public delegate void PanelPressedEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelUnpressedEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelMousedOverEventHandler(DisplayResource resource);
    [Signal] public delegate void PanelMousedAwayEventHandler();

    private StyleBox _normal { get; set; }
    private StyleBox _pressed { get; set; }
    private StyleBox _hover { get; set; }

    [ExportGroup("Styles")]
    [Export] private StyleBox Normal
    {
      get => _normal;
        set
        {
            _normal = value;
            Panels.ToList()
                .ForEach(SetPanelStyle);
        }  
    }
    [Export] private StyleBox Pressed
    {
      get => _pressed;
        set
        {
            _pressed = value;
            Panels.ToList()
                .ForEach(SetPanelStyle);
        }  
    }
    [Export] private StyleBox Hover
    {
      get => _hover;
        set
        {
            _hover = value;
            Panels.ToList()
                .ForEach(SetPanelStyle);
        }  
    }

    public System.Collections.Generic.List<DisplayResource> Resources =>
        Panels.Select(panel => panel.Resource).ToList();

    private System.Collections.Generic.List<DisplayPanel> Panels =>
        GetChildren()
            .Where(child => child is DisplayPanel)
            .Cast<DisplayPanel>().ToList();

    public override void _Ready()
    {
        base._Ready();
        ChildEnteredTree += OnResourceAdded;
    }

    public void Populate(System.Collections.Generic.List<DisplayResource> resources)
    {
        GetChildren().ToList()
            .ForEach(child =>
            {
                RemoveChild(child);
                child.QueueFree();
            });
        foreach (var resource in resources)
        {
            var panel = new DisplayPanel
            {
                Resource = resource
            };
            AddChild(panel);
        }
    }

    private void OnPressed(DisplayResource resource, DisplayPanel panel)
    {
        Panels
            .Where(other => other != panel)
            .Where(other => other.IsPressed).ToList()
            .ForEach(other => other.IsPressed = !other.IsPressed);
        EmitSignal(SignalName.PanelPressed, resource);
    }
        

    private void OnUnpressed(DisplayResource resource) =>
        EmitSignal(SignalName.PanelUnpressed, resource);

    private void OnMousedOver(DisplayResource resource) =>
        EmitSignal(SignalName.PanelMousedOver, resource);

    private void OnMousedAway() =>
        EmitSignal(SignalName.PanelMousedAway);

    private void OnResourceAdded(Node node)
    {
        if (node is not DisplayPanel panel)
        {
            RemoveChild(node);
            node.QueueFree();
            return;
        }
        panel.PanelPressed += (DisplayResource resource) => OnPressed(resource, panel);
        panel.PanelUnpressed += OnUnpressed;
        panel.PanelMousedOver += OnMousedOver;
        panel.PanelMousedAway += OnMousedAway;
        SetPanelStyle(panel);
        if (Normal is not null) panel.AddThemeStyleboxOverride("panel", Normal);
    }

    private void SetPanelStyle(Node node)
    {
        if (node is not DisplayPanel panel) return;
        panel.SetStyle("normal", Normal);
        panel.SetStyle("pressed", Pressed);
        panel.SetStyle("hover", Hover);
    }
}
