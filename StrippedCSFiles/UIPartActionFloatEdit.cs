using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine.UI;

[UI_FloatEdit]
public class UIPartActionFloatEdit : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle incLarge;

	public UIButtonToggle incSmall;

	public UIButtonToggle decLarge;

	public UIButtonToggle decSmall;

	public Slider slider;

	private bool blockSliderUpdate;

	protected UI_FloatEdit floatControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFloatEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float UpdateSlider(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float IntervalBase(float value, float increment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SliderInterval(float value, out float min, out float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateControlStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplay(float value, UIButtonToggle button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float Clamp(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float AdjustValue(float value, bool up, float increment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_incLarge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_incSmall()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_decLarge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_decSmall()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnValueChanged(float obj)
	{
		throw null;
	}
}
