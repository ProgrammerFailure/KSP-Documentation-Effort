using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorImage : MonoBehaviour
{
	public ColorPicker picker;

	public Image image;

	public void Awake()
	{
		image = GetComponent<Image>();
		picker.onValueChanged.AddListener(ColorChanged);
		picker.onInternalValueChanged.AddListener(ColorChanged);
	}

	public void OnDestroy()
	{
		picker.onValueChanged.RemoveListener(ColorChanged);
		picker.onInternalValueChanged.RemoveListener(ColorChanged);
	}

	public void ColorChanged(Color newColor)
	{
		image.color = newColor;
	}
}
