using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class HexColorField : MonoBehaviour
{
	public ColorPicker hsvpicker;

	public bool displayAlpha;

	private TMP_InputField hexInputField;

	private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HexColorField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateHex(Color newColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateColor(string newHex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ColorToHex(Color32 color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HexToColor(string hex, out Color32 color)
	{
		throw null;
	}
}
