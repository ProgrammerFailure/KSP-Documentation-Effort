using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;

[UI_Label]
public class UIPartActionFuelFlowOverlay : UIPartActionItem
{
	[SerializeField]
	private UIButtonToggle flowToggle;

	[SerializeField]
	private TextMeshProUGUI flowText;

	private bool isConsumer;

	private bool isProvider;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFuelFlowOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlowToggle()
	{
		throw null;
	}
}
