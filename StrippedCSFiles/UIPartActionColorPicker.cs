using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_ColorPicker]
public class UIPartActionColorPicker : UIPartActionFieldItem
{
	public string id;

	public UI_ColorPicker UI_colorPicker;

	public Button colorPickerButton;

	public ColorPicker colorPicker;

	public ColorPresets colorPresets;

	public Image currentColorImage;

	public TextMeshProUGUI fieldNameText;

	public GameObject colorSelectWindow;

	public Transform colorPresetLayout;

	public GameObject colorPresetPrefab;

	public bool showingColorSelectWindow;

	private bool useFieldNameForColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionColorPicker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleColorPickWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseColorPickWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeColourPresets(List<Color> presetColour)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnColorChange(Color color)
	{
		throw null;
	}
}
