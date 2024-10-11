using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine.UI;

[UI_Toggle]
public class UIPartActionToggle : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldStatus;

	public UIButtonToggle toggle;

	public TextMeshProUGUI tipText;

	private LayoutElement layoutElement;

	public float heightNoTip;

	public float heightWithTip;

	private bool fieldValue;

	protected UI_Toggle toggleControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetButtonState(bool state, bool forceButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}
}
