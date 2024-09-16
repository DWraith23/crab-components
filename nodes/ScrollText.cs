using Godot;
using System.Linq;

namespace Crab.Nodes;

[Tool]
public partial class ScrollText : ScrollContainer
{
    private string _text = string.Empty;
    private int _fontSize { get; set; } = 12;

    [Export]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Label.Text = value;
        }
    }

    [Export]
    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            Label.AddThemeFontSizeOverride("font_size", value);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        GetChildren().ToList()
            .ForEach(RemoveChild);
        Label.AddThemeFontSizeOverride("font_size", FontSize);
        Label.CustomMinimumSize = new Vector2(Size.X, Size.Y);
        AddChild(Label, false, InternalMode.Front);
        Resized += ResizeLabel;
    }

    public void SetText(string text) => Text = text;
    public void SetFontColor(Color color) => Label.AddThemeColorOverride("font_color", color);

    private Label Label {get; set; } = new();

    private void ResizeLabel() => Label.CustomMinimumSize = new Vector2(Size.X, Size.Y);
}
