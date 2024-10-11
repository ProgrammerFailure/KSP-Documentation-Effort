using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine.UI;

public class UIPartActionResource : UIPartActionResourceItem
{
	public TextMeshProUGUI resourceName;

	public TextMeshProUGUI resourceAmnt;

	public TextMeshProUGUI resourceMax;

	public Slider progBar;

	public UIButtonToggle flowBtn;

	private int displayNameLimit;

	private TooltipController_Text tooltip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsItemValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FlowBtnToggled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetButtonState(bool state, bool forceButton)
	{
		throw null;
	}
}
