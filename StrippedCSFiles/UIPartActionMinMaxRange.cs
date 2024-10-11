using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[UI_MinMaxRange]
public class UIPartActionMinMaxRange : UIPartActionFieldItem
{
	public GameObject slidersContainer;

	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldAmount;

	public GameObject numericContainer;

	public TextMeshProUGUI fieldNameNumeric;

	public TMP_InputField inputFieldMin;

	public TMP_InputField inputFieldMax;

	public DoubleSlider slider;

	private Vector2 fieldValue;

	private float lerpedValueMin;

	private float moddedValueMin;

	private float lerpedValueMax;

	private float moddedValueMax;

	private bool handlingChange;

	protected UI_MinMaxRange progBarControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionMinMaxRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChanged(float minValue, float maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSliderValue(Vector2 rawValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetNumericMinValue(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetNumericMaxValue(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleNumericSlider(bool numeric)
	{
		throw null;
	}
}
