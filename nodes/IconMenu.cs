using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Crab.Nodes;

[Tool]
public partial class IconMenu : FlowContainer
{
	[Signal] public delegate void IconPressedEventHandler(int index);

	private Texture2D[] _icons;
	private int _iconSize;
	private int _iconSpacing;
	private Color _borderColorNormal;
	private Color _borderColorHover;
	private Color _borderColorPressed;
	private int _borderWidth;
	private int _borderCornerRadius;
	private bool _borderBlend;


	#region Exports
	[Export]
	private Texture2D[] Icons
	{
		get => _icons;
		set
		{
			_icons = value;
			UpdateIcons(value);
		}
	}

	[ExportGroup("Options")]
	[Export]
	private int IconSize
	{
		get => _iconSize;
		set
		{
			SetIconSize(value);
		}
	}

	[Export]
	private int IconSpacing
	{
		get => _iconSpacing;
		set
		{
			SetIconSpacing(value);
		}
	}

	[Export]
	private Color BorderColorNormal
	{
		get => _borderColorNormal;
		set
		{
			SetBorderColor("normal", value);
		}
	}

	[Export]
	private Color BorderColorPressed
	{
		get => _borderColorPressed;
		set
		{
			SetBorderColor("pressed", value);
		}
	}

	[Export]
	private Color BorderColorHover
	{
		get => _borderColorHover;
		set
		{
			SetBorderColor("hover", value);
		}
	}

	[Export]
	private int BorderWidth
	{
		get => _borderWidth;
		set
		{
			SetBorderWidth(value);
		}
	}

	[Export]
	private int BorderCornerRadius
	{
		get => _borderCornerRadius;
		set
		{
			SetBorderCornerRadius(value);
		}
	}

	[Export]
	private bool BorderBlend
	{
		get => _borderBlend;
		set
		{
			SetBorderBlend(value);
		}
	} 

	#endregion

    public override void _Ready()
    {
        base._Ready();
		UpdateIcons(_icons);
    }


    private void UpdateIcons(Texture2D[] icons)
	{
		GetChildren(true).ToList()
               .ForEach(child =>
			   {
				if (child.GetParent() is not null) child.GetParent().RemoveChild(child);
				child.QueueFree();
			   });
		foreach(var icon in icons)
		{
			var panel = CreateIconButtonPanel();
			var button = CreateIconButton(icon);
			AddChild(panel, false, InternalMode.Front);
			panel.AddChild(button, false, InternalMode.Front);
		}
	}

	private TextureButton CreateIconButton(Texture2D icon)
	{
		var button = new TextureButton()
		{
			TextureNormal = icon,
			CustomMinimumSize = new Vector2(_iconSize, _iconSize),
			IgnoreTextureSize = true,
			StretchMode = TextureButton.StretchModeEnum.Scale,
		};
		var count = GetChildCount(true);
		button.Pressed += () => OnButtonPressed(count, button);
		button.MouseEntered += () => OnButtonMousedOver(button);
		button.MouseExited += () => OnButtonMousedAway(button);

		return button;
	}

	private PanelContainer CreateIconButtonPanel()
	{
		var stylebox = new StyleBoxFlat()
		{
			DrawCenter = false,
			BorderColor = _borderColorNormal,
			BorderWidthBottom = _borderWidth,
			BorderWidthLeft = _borderWidth,
			BorderWidthRight = _borderWidth,
			BorderWidthTop = _borderWidth,
			BorderBlend = _borderBlend,
			CornerRadiusBottomLeft = _borderCornerRadius,
			CornerRadiusBottomRight = _borderCornerRadius,
			CornerRadiusTopLeft = _borderCornerRadius,
			CornerRadiusTopRight = _borderCornerRadius,
			ContentMarginBottom = 8,
			ContentMarginLeft = 8,
			ContentMarginRight = 8,
			ContentMarginTop = 8,
		};

		var panel = new PanelContainer();
		panel.AddThemeStyleboxOverride("panel", stylebox);
		return panel;
	}

	private static void UpdateBorderColor(PanelContainer panel, Color color)
	{
		var stylebox = panel.GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
		stylebox.BorderColor = color;
		panel.AddThemeStyleboxOverride("panel", stylebox);
	}

	#region Signal Methods
	private async void OnButtonPressed(int index, TextureButton button)
	{
		var panel = button.GetParent<PanelContainer>();
		UpdateBorderColor(panel, _borderColorPressed);
		EmitSignal(SignalName.IconPressed, index);
		await Task.Delay(250);
		if (panel is not null && IsInstanceValid(panel)) UpdateBorderColor(panel, _borderColorHover);
	}

	private void OnButtonMousedOver(TextureButton button)
	{
		var panel = button.GetParent<PanelContainer>();
		UpdateBorderColor(panel, _borderColorHover);
	}

	private void OnButtonMousedAway(TextureButton button)
	{
		var panel = button.GetParent<PanelContainer>();
		UpdateBorderColor(panel, _borderColorNormal);
	}
	#endregion

	#region Set Methods

	public void SetIconSize(int value)
	{
		_iconSize = value;
		foreach (var child in GetChildren(true).OfType<PanelContainer>())
		{
			child.GetChild<TextureButton>(0, true).CustomMinimumSize = new Vector2(value, value);
		}
	}

	public void SetIconSpacing(int value)
	{
		_iconSpacing = value;
		AddThemeConstantOverride("h_separation", value);
		AddThemeConstantOverride("v_separation", value);
	}

	public void SetBorderColor(string borderType, Color color)
	{
		switch (borderType)
		{
			case "normal" :
				_borderColorNormal = color;
				GetChildren(true).OfType<PanelContainer>().ToList()
					.ForEach(child => UpdateBorderColor(child, _borderColorNormal));
				break;
			case "pressed" : _borderColorPressed = color; break;
			case "hover" : _borderColorHover = color; break;
			default : break;
		}
	}

	public void SetBorderWidth(int value)
	{
		_borderWidth = value;
		foreach (var child in GetChildren(true).OfType<PanelContainer>())
		{
			var stylebox = child.GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
			stylebox.BorderWidthBottom = value;
			stylebox.BorderWidthLeft = value;
			stylebox.BorderWidthRight = value;
			stylebox.BorderWidthTop = value;
			child.AddThemeStyleboxOverride("panel", stylebox);
		}
	}

	public void SetBorderCornerRadius(int value)
	{
		_borderCornerRadius = value;
		foreach (var child in GetChildren(true).OfType<PanelContainer>())
		{
			var stylebox = child.GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
			stylebox.CornerRadiusBottomLeft = value;
			stylebox.CornerRadiusBottomRight = value;
			stylebox.CornerRadiusTopLeft = value;
			stylebox.CornerRadiusTopRight = value;
			child.AddThemeStyleboxOverride("panel", stylebox);
		}

	}
	public void SetBorderBlend(bool value)
	{
		_borderBlend = value;
		foreach (var child in GetChildren(true).OfType<PanelContainer>())
		{
			var stylebox = child.GetThemeStylebox("panel").Duplicate(true) as StyleBoxFlat;
			stylebox.BorderBlend = value;
			child.AddThemeStyleboxOverride("panel", stylebox);
		}
	}
	#endregion
}
