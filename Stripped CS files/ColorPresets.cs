using UnityEngine;
using UnityEngine.UI;

public class ColorPresets : MonoBehaviour
{
	public ColorPicker picker;

	public GameObject[] presets;

	public Image createPresetImage;

	public void Awake()
	{
		picker.onValueChanged.AddListener(ColorChanged);
	}

	public void CreatePresetButton()
	{
		int num = 0;
		while (true)
		{
			if (num < presets.Length)
			{
				if (!presets[num].activeSelf)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		presets[num].SetActive(value: true);
		presets[num].GetComponent<Image>().color = picker.CurrentColor;
	}

	public void ColorChanged(Color color)
	{
		if (createPresetImage != null)
		{
			createPresetImage.color = color;
		}
	}

	public void OnPresetSelected(Image image)
	{
		if (image != null && picker != null)
		{
			picker.CurrentColor = image.color;
		}
	}
}
