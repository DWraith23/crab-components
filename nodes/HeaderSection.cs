using Godot;

namespace Crab.Nodes;

[Tool]
public partial class HeaderSection : VBoxContainer
{
    private string _headerText { get; set; }

    [Export]
    public string HeaderText
    {
        get => _headerText;
        set
        {
            _headerText = value;
            if (Header is not null)
            {
                Header.Text = value;
            }
                
        }
    }

    private Label Header {get; set; }
    private HSeparator Seperator => new();

    public override void _Ready()
    {
        base._Ready();
        Header = new Label();
        Header.Text = HeaderText;
        Header.AddThemeFontSizeOverride("font_size", 24);
        Header.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        AddChild(Header, false, InternalMode.Front);
        AddChild(Seperator, false, InternalMode.Front);
    }

    public override void _EnterTree()
    {
        base._EnterTree();

    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

}
