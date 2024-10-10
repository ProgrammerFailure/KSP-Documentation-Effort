using ns11;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class GAPUtil_VesselFrame : GAPUtil_BiSelector
{
	public delegate void categoryValueChange(PartCategories category, bool state);

	public categoryValueChange onCategoryValueChange;

	public float scrollStep = 0.1f;

	[SerializeField]
	public ScrollRect partCategoryScroll;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnDown;

	[SerializeField]
	public PointerClickAndHoldHandler GetScrollBtnUp;

	public override void Awake()
	{
		base.Awake();
		onCategoryValueChange = delegate
		{
		};
		GetScrollBtnDown.onPointerDownHold.AddListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.AddListener(OnPointerUp);
	}

	public void SetPartTypes(ShipConstruct vessel)
	{
		DictionaryValueList<PartCategories, int> dictionaryValueList = new DictionaryValueList<PartCategories, int>();
		if (vessel != null)
		{
			for (int i = 0; i < vessel.Parts.Count; i++)
			{
				if (dictionaryValueList.ContainsKey(vessel.Parts[i].partInfo.category))
				{
					dictionaryValueList[vessel.Parts[i].partInfo.category]++;
				}
				else
				{
					dictionaryValueList.Add(vessel.Parts[i].partInfo.category, 1);
				}
			}
		}
		int j = 0;
		for (int count = dictionaryValueList.Count; j < count; j++)
		{
			MEPartCategoryButton gapControl = AddSidebarGAPButton(dictionaryValueList.KeyAt(j).ToString(), dictionaryValueList.KeyAt(j).ToString() + "Icon", dictionaryValueList.KeyAt(j).Description(), startState: true, dictionaryValueList.At(j));
			PartCategories category = dictionaryValueList.KeyAt(j);
			gapControl.button.onClick.AddListener(delegate
			{
				OnCategoryValueChange(category, gapControl);
			});
		}
	}

	public void SetTitleText(string newText, bool playerCreated)
	{
		titleText.text = Localizer.Format(newText) + (playerCreated ? Localizer.Format("#autoLOC_8006117") : "");
	}

	public void OnCategoryValueChange(PartCategories category, MEPartCategoryButton control)
	{
		if (onCategoryValueChange != null)
		{
			onCategoryValueChange(category, control.state);
		}
	}

	public void ScrollWithButtons(float direction)
	{
		partCategoryScroll.verticalNormalizedPosition = Mathf.Clamp(partCategoryScroll.verticalNormalizedPosition + scrollStep * direction, 0f, 1f);
	}

	public void OnPointerDown(PointerEventData data)
	{
		ScrollWithButtons(-1f);
	}

	public void OnPointerUp(PointerEventData data)
	{
		ScrollWithButtons(1f);
	}

	public void OnDestroy()
	{
		GetScrollBtnDown.onPointerDownHold.RemoveListener(OnPointerDown);
		GetScrollBtnUp.onPointerDownHold.RemoveListener(OnPointerDown);
	}
}
