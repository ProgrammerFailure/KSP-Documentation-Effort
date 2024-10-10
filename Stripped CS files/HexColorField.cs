using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class HexColorField : MonoBehaviour
{
	public ColorPicker hsvpicker;

	public bool displayAlpha;

	public TMP_InputField hexInputField;

	public const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";

	public void Start()
	{
		hexInputField = GetComponent<TMP_InputField>();
		UpdateHex(hsvpicker.CurrentColor);
		hexInputField.onEndEdit.AddListener(UpdateColor);
		hsvpicker.onValueChanged.AddListener(UpdateHex);
		hsvpicker.onInternalValueChanged.AddListener(UpdateHex);
	}

	public void OnDestroy()
	{
		if (hexInputField != null)
		{
			hexInputField.onValueChanged.RemoveListener(UpdateColor);
		}
		hsvpicker.onValueChanged.RemoveListener(UpdateHex);
		hsvpicker.onInternalValueChanged.RemoveListener(UpdateHex);
	}

	public void UpdateHex(Color newColor)
	{
		hexInputField.text = ColorToHex(newColor);
	}

	public void UpdateColor(string newHex)
	{
		if (HexToColor(newHex, out var color))
		{
			hsvpicker.CurrentColor = color;
		}
		else
		{
			Debug.Log("hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional)");
		}
	}

	public string ColorToHex(Color32 color)
	{
		if (displayAlpha)
		{
			return $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
		}
		return $"#{color.r:X2}{color.g:X2}{color.b:X2}";
	}

	public static bool HexToColor(string hex, out Color32 color)
	{
		if (Regex.IsMatch(hex, "^#?(?:[0-9a-fA-F]{3,4}){1,2}$"))
		{
			int num = (hex.StartsWith("#") ? 1 : 0);
			if (hex.Length == num + 8)
			{
				color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier));
			}
			else if (hex.Length == num + 6)
			{
				color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.MaxValue);
			}
			else if (hex.Length == num + 4)
			{
				color = new Color32(byte.Parse(hex[num].ToString() + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 3].ToString() + hex[num + 3], NumberStyles.AllowHexSpecifier));
			}
			else
			{
				color = new Color32(byte.Parse(hex[num].ToString() + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.MaxValue);
			}
			return true;
		}
		color = default(Color32);
		return false;
	}
}
