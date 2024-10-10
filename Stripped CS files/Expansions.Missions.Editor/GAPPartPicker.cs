using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPPartPicker : ActionPaneDisplay
{
	public MEPartSelectorBrowser partSelector;

	public void Setup(MEGUIParameterPartPicker partPicker, MEPartSelectorBrowser.PartSelectionCallback onSelectionChange)
	{
		partSelector = GetComponentInChildren<MEPartSelectorBrowser>(includeInactive: true);
		if (partSelector == null)
		{
			partSelector = partPicker.partSelectorPrefab.Spawn(partPicker, onSelectionChange, rectTransform);
			RectTransform obj = partSelector.transform as RectTransform;
			obj.anchorMax = Vector2.one;
			obj.anchorMin = Vector2.zero;
			Vector2 offsetMin = (obj.offsetMax = Vector2.zero);
			obj.offsetMin = offsetMin;
		}
		else
		{
			partSelector.gameObject.SetActive(value: true);
			partSelector.Setup(partPicker, onSelectionChange);
		}
	}

	public override void Clean()
	{
		base.Clean();
		if (partSelector != null)
		{
			partSelector.gameObject.SetActive(value: false);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (partSelector != null)
		{
			Object.Destroy(partSelector.gameObject);
		}
	}
}
