using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManeuverNodeEditorTabButton : MonoBehaviour
{
	public Toggle toggle;

	public TooltipController_Text tooltip;

	public Image background;

	public Image iconOff;

	public Image iconOn;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Transform tabButtonsParent, ManeuverNodeEditorTab tab, UnityAction<bool> toggleAction)
	{
		throw null;
	}
}
