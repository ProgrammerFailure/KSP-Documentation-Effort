using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ColorLabel : MonoBehaviour
{
	public ColorPicker picker;

	public ColorValues type;

	public string prefix;

	public float minValue;

	public float maxValue;

	public int precision;

	private TextMeshProUGUI label;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ColorLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
	private void ColorChanged(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HSVChanged(float hue, float sateration, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ConvertToDisplayString(float value)
	{
		throw null;
	}
}
