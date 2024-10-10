using System.Collections.Generic;
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

	public bool useFieldNameForColor;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		useFieldNameForColor = ((UI_ColorPicker)control).useFieldNameForColor;
		if (colorPickerButton != null && colorSelectWindow != null)
		{
			colorPickerButton.onClick.AddListener(ToggleColorPickWindow);
		}
		if (field != null && fieldNameText != null)
		{
			fieldNameText.text = field.guiName;
			id = field.name;
		}
		if ((scene == UI_Scene.Flight || scene == UI_Scene.Editor) && UI_colorPicker == null)
		{
			UI_colorPicker = (UI_ColorPicker)control;
			UI_colorPicker.UI_PartActionColorPicker = this;
		}
		if (partModule != null)
		{
			InitializeColourPresets(partModule.PresetColors());
			colorPicker.CurrentColor = partModule.GetCurrentColor();
			if (!useFieldNameForColor)
			{
				currentColorImage.color = partModule.GetCurrentColor();
			}
			else
			{
				currentColorImage.color = partModule.GetCurrentColor(field.name);
			}
		}
		colorPicker.onValueChanged.AddListener(OnColorChange);
	}

	public void OnEnable()
	{
		if (colorSelectWindow.activeInHierarchy)
		{
			ToggleColorPickWindow();
		}
	}

	public void OnDestroy()
	{
		if (colorPickerButton != null && colorSelectWindow != null)
		{
			colorPickerButton.onClick.RemoveListener(ToggleColorPickWindow);
		}
		colorPicker.onValueChanged.RemoveListener(OnColorChange);
	}

	public void ToggleColorPickWindow()
	{
		window.CloseAllColorPickers(this);
		if (!(colorSelectWindow == null))
		{
			bool active = !colorSelectWindow.activeInHierarchy;
			colorSelectWindow.SetActive(active);
			showingColorSelectWindow = active;
		}
	}

	public void CloseColorPickWindow()
	{
		if (!(colorSelectWindow == null))
		{
			colorSelectWindow.SetActive(value: false);
			showingColorSelectWindow = false;
		}
	}

	public void InitializeColourPresets(List<Color> presetColour)
	{
		if (colorPresetLayout == null || colorPresetPrefab == null)
		{
			return;
		}
		for (int i = 0; i < presetColour.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(colorPresetPrefab, colorPresetLayout);
			Image image = gameObject.GetComponent<Image>();
			Button component = gameObject.GetComponent<Button>();
			if (colorPresets != null && component != null && image != null)
			{
				image.color = presetColour[i];
				component.onClick.AddListener(delegate
				{
					OnColorChange(image.color);
				});
			}
		}
	}

	public void OnColorChange(Color color)
	{
		color.a = 1f;
		currentColorImage.color = color;
		if (!(partModule != null))
		{
			return;
		}
		partModule.OnColorChanged(color, id);
		for (int i = 0; i < partModule.part.symmetryCounterparts.Count; i++)
		{
			for (int j = 0; j < partModule.part.symmetryCounterparts[i].Modules.Count; j++)
			{
				partModule.part.symmetryCounterparts[i].Modules[j].OnColorChanged(color, id);
			}
		}
	}
}
