using System.Runtime.CompilerServices;
using UnityEngine.UI;

[UI_FieldFloatRange]
public class UIPartActionFieldFloatRange : UIPartActionFieldItem
{
	public Text fieldName;

	public Text fieldAmount;

	public Slider slider;

	private BaseField minValueField;

	private float minValue;

	private BaseField maxValueField;

	private float maxValue;

	private float fieldValue;

	private bool handlingChange;

	protected UI_FieldFloatRange progBarControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFieldFloatRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFieldValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSliderValue(float rawValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChanged(float obj)
	{
		throw null;
	}
}
