using ns12;
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

	public void Setup(Transform tabButtonsParent, ManeuverNodeEditorTab tab, UnityAction<bool> toggleAction)
	{
		base.transform.SetParent(tabButtonsParent);
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		iconOff.sprite = tab.tabIconOff;
		iconOn.sprite = tab.tabIconOn;
		tooltip.textString = tab.tabTooltipCaptionActive;
		toggle.group = tabButtonsParent.GetComponent<ToggleGroup>();
		toggle.onValueChanged.AddListener(toggleAction);
	}
}
