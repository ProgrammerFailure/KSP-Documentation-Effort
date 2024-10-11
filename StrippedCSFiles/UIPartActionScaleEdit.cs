using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine.UI;

[UI_ScaleEdit]
public class UIPartActionScaleEdit : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle inc;

	public UIButtonToggle dec;

	public Slider slider;

	private int intervalIndex;

	protected UI_ScaleEdit scaleControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionScaleEdit()
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
	private int FindInterval(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float UpdateSlider(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplay(float value, UIButtonToggle button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInterval(bool up, UIButtonToggle button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_inc()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap_dec()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnValueChanged(float obj)
	{
		throw null;
	}
}
