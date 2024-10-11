using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine.UI;

[UI_ChooseOption]
public class UIPartActionChooseOption : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldValue;

	public UIButtonToggle inc;

	public UIButtonToggle dec;

	public Slider slider;

	private int index;

	protected UI_ChooseOption chooserControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionChooseOption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFieldValue(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetDisplay(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplay(UIButtonToggle button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateState(int dir, UIButtonToggle button)
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
