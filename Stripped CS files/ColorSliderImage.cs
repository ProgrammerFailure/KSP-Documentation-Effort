using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class ColorSliderImage : MonoBehaviour
{
	public ColorPicker picker;

	public ColorValues type;

	public Slider.Direction direction;

	public RawImage image;

	public RectTransform rectTransform => base.transform as RectTransform;

	public void Awake()
	{
		image = GetComponent<RawImage>();
		RegenerateTexture();
	}

	public void OnEnable()
	{
		if (picker != null && Application.isPlaying)
		{
			picker.onValueChanged.AddListener(ColorChanged);
			picker.onInternalValueChanged.AddListener(ColorChanged);
			picker.onHSVChanged.AddListener(HSVChanged);
			ColorChanged(picker.CurrentColor);
			HSVChanged(picker.Single_0, picker.Single_1, picker.Single_2);
		}
	}

	public void OnDisable()
	{
		if (picker != null)
		{
			picker.onValueChanged.RemoveListener(ColorChanged);
			picker.onInternalValueChanged.RemoveListener(ColorChanged);
			picker.onHSVChanged.RemoveListener(HSVChanged);
		}
	}

	public void OnDestroy()
	{
		if (image.texture != null)
		{
			UnityEngine.Object.DestroyImmediate(image.texture);
		}
	}

	public void ColorChanged(Color newColor)
	{
		switch (type)
		{
		case ColorValues.const_0:
		case ColorValues.const_1:
		case ColorValues.const_2:
		case ColorValues.Saturation:
		case ColorValues.Value:
			RegenerateTexture();
			break;
		case ColorValues.const_3:
		case ColorValues.Hue:
			break;
		}
	}

	public void HSVChanged(float hue, float saturation, float value)
	{
		switch (type)
		{
		case ColorValues.const_0:
		case ColorValues.const_1:
		case ColorValues.const_2:
		case ColorValues.Saturation:
		case ColorValues.Value:
			RegenerateTexture();
			break;
		case ColorValues.const_3:
		case ColorValues.Hue:
			break;
		}
	}

	public void RegenerateTexture()
	{
		Color32 color = ((picker != null) ? picker.CurrentColor : Color.black);
		float num = ((picker != null) ? picker.Single_0 : 0f);
		float num2 = ((picker != null) ? picker.Single_1 : 0f);
		float num3 = ((picker != null) ? picker.Single_2 : 0f);
		bool flag = direction == Slider.Direction.BottomToTop || direction == Slider.Direction.TopToBottom;
		bool flag2 = direction == Slider.Direction.TopToBottom || direction == Slider.Direction.RightToLeft;
		int num4;
		switch (type)
		{
		default:
			throw new NotImplementedException("");
		case ColorValues.const_0:
		case ColorValues.const_1:
		case ColorValues.const_2:
		case ColorValues.const_3:
			num4 = 255;
			break;
		case ColorValues.Hue:
			num4 = 360;
			break;
		case ColorValues.Saturation:
		case ColorValues.Value:
			num4 = 100;
			break;
		}
		Texture2D texture2D = ((!flag) ? new Texture2D(num4, 1) : new Texture2D(1, num4));
		texture2D.hideFlags = HideFlags.DontSave;
		Color32[] array = new Color32[num4];
		switch (type)
		{
		default:
			throw new NotImplementedException("");
		case ColorValues.const_0:
		{
			for (byte b2 = 0; b2 < num4; b2++)
			{
				array[flag2 ? (num4 - 1 - b2) : b2] = new Color32(b2, color.g, color.b, byte.MaxValue);
			}
			break;
		}
		case ColorValues.const_1:
		{
			for (byte b3 = 0; b3 < num4; b3++)
			{
				array[flag2 ? (num4 - 1 - b3) : b3] = new Color32(color.r, b3, color.b, byte.MaxValue);
			}
			break;
		}
		case ColorValues.const_2:
		{
			for (byte b = 0; b < num4; b++)
			{
				array[flag2 ? (num4 - 1 - b) : b] = new Color32(color.r, color.g, b, byte.MaxValue);
			}
			break;
		}
		case ColorValues.const_3:
		{
			for (byte b4 = 0; b4 < num4; b4++)
			{
				array[flag2 ? (num4 - 1 - b4) : b4] = new Color32(b4, b4, b4, byte.MaxValue);
			}
			break;
		}
		case ColorValues.Hue:
		{
			for (int k = 0; k < num4; k++)
			{
				array[flag2 ? (num4 - 1 - k) : k] = HSVUtil.ConvertHsvToRgb(k, 1.0, 1.0, 1f);
			}
			break;
		}
		case ColorValues.Saturation:
		{
			for (int j = 0; j < num4; j++)
			{
				array[flag2 ? (num4 - 1 - j) : j] = HSVUtil.ConvertHsvToRgb(num * 360f, (float)j / (float)num4, num3, 1f);
			}
			break;
		}
		case ColorValues.Value:
		{
			for (int i = 0; i < num4; i++)
			{
				array[flag2 ? (num4 - 1 - i) : i] = HSVUtil.ConvertHsvToRgb(num * 360f, num2, (float)i / (float)num4, 1f);
			}
			break;
		}
		}
		texture2D.SetPixels32(array);
		texture2D.Apply();
		if (image.texture != null)
		{
			UnityEngine.Object.DestroyImmediate(image.texture);
		}
		image.texture = texture2D;
		switch (direction)
		{
		case Slider.Direction.BottomToTop:
		case Slider.Direction.TopToBottom:
			image.uvRect = new Rect(0f, 0f, 2f, 1f);
			break;
		case Slider.Direction.LeftToRight:
		case Slider.Direction.RightToLeft:
			image.uvRect = new Rect(0f, 0f, 1f, 2f);
			break;
		}
	}
}
