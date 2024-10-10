using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_VariantSelector]
public class UIPartActionVariantSelector : UIPartActionFieldItem
{
	public GameObject prefabVariantButton;

	public Button buttonPrevious;

	public Button buttonNext;

	public ScrollRect scrollMain;

	public TextMeshProUGUI variantName;

	public int fieldValue;

	public UI_VariantSelector variantSelector;

	public List<UIPartActionVariantButton> buttonList;

	public List<int> lockedButtonIndexes;

	public bool hadSurfaceAttachedParts;

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		buttonNext.onClick.AddListener(OnButtonNext);
		buttonPrevious.onClick.AddListener(OnButtonPrev);
		fieldValue = GetFieldValue();
		RefreshVariantButtons();
		if (variantSelector == null)
		{
			variantSelector = (UI_VariantSelector)control;
		}
		GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
	}

	public void OnDestroy()
	{
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		if ((evt == ConstructionEventType.PartAttached || evt == ConstructionEventType.PartDetached) && !(part == null) && GetSurfaceAttachedPartCount() > 0 != hadSurfaceAttachedParts)
		{
			RefreshVariantButtons();
		}
	}

	public override void UpdateItem()
	{
		base.transform.SetAsLastSibling();
	}

	public void RefreshVariantButtons()
	{
		buttonList = new List<UIPartActionVariantButton>();
		lockedButtonIndexes = new List<int>();
		int childCount = scrollMain.content.childCount;
		while (childCount-- > 0)
		{
			if (scrollMain.content.GetChild(childCount).name != "Space")
			{
				Object.Destroy(scrollMain.content.GetChild(childCount).gameObject);
			}
		}
		if (variantSelector == null)
		{
			variantSelector = (UI_VariantSelector)control;
		}
		if (variantSelector != null && variantSelector.variants != null)
		{
			for (int i = 0; i < variantSelector.variants.Count; i++)
			{
				string hexSecondaryColor = variantSelector.variants[i].PrimaryColor;
				if (!string.IsNullOrEmpty(variantSelector.variants[i].SecondaryColor))
				{
					hexSecondaryColor = variantSelector.variants[i].SecondaryColor;
				}
				UIPartActionVariantButton uIPartActionVariantButton = AddVariantButton(i, variantSelector.variants[i].PrimaryColor, hexSecondaryColor);
				if (GetSurfaceAttachedPartCount() > 0)
				{
					uIPartActionVariantButton.Locked = variantSelector.variants[i].SizeGroup != variantSelector.variants[fieldValue].SizeGroup;
					if (uIPartActionVariantButton.Locked)
					{
						lockedButtonIndexes.Add(i);
					}
				}
				buttonList.Add(uIPartActionVariantButton);
			}
			if (buttonList.Count > fieldValue)
			{
				buttonList[fieldValue].Select();
				variantName.text = variantSelector.variants[fieldValue].DisplayName;
			}
			hadSurfaceAttachedParts = GetSurfaceAttachedPartCount() > 0;
		}
		else
		{
			Debug.Log("UIPartActionVariantSelector: Could not find any variants - on part " + part.name);
		}
	}

	public int GetSurfaceAttachedPartCount()
	{
		int num = 0;
		for (int i = 0; i < part.transform.childCount; i++)
		{
			Part component = part.transform.GetChild(i).GetComponent<Part>();
			if (component != null && component.attachMode == AttachModes.SRF_ATTACH)
			{
				num++;
			}
		}
		return num;
	}

	public UIPartActionVariantButton AddVariantButton(int entryIndex, string hexPrimaryColor, string hexSecondaryColor)
	{
		UIPartActionVariantButton component = Object.Instantiate(prefabVariantButton, scrollMain.content).GetComponent<UIPartActionVariantButton>();
		component.Setup(this, entryIndex, hexPrimaryColor, hexSecondaryColor);
		return component;
	}

	public int GetFieldValue()
	{
		return field.GetValue<int>(field.host);
	}

	public void OnButtonPressed(int newIndex)
	{
		SelectVariant(newIndex);
	}

	public void SetButtonOverText(int entryIndex)
	{
		if (entryIndex != fieldValue)
		{
			string text = Localizer.Format(variantSelector.variants[entryIndex].DisplayName);
			if (buttonList[entryIndex].Locked)
			{
				variantName.text = Localizer.Format("#autoLOC_8007003", text);
			}
			else
			{
				variantName.text = Localizer.Format("#autoLOC_8007004", text);
			}
		}
		else
		{
			variantName.text = variantSelector.variants[fieldValue].DisplayName;
		}
	}

	public void ResetButtonOverText()
	{
		variantName.text = variantSelector.variants[fieldValue].DisplayName;
	}

	public void OnButtonNext()
	{
		int num = fieldValue;
		int num2 = num;
		do
		{
			num2 = (num2 + 1) % variantSelector.variants.Count;
		}
		while (num2 != num && lockedButtonIndexes.Contains(num2));
		SelectVariant(num2);
	}

	public void OnButtonPrev()
	{
		int num = fieldValue;
		int num2 = num;
		do
		{
			num2 = (num2 + variantSelector.variants.Count - 1) % variantSelector.variants.Count;
		}
		while (num2 != num && lockedButtonIndexes.Contains(num2));
		SelectVariant(num2);
	}

	public bool SelectVariant(int newIndex)
	{
		buttonList[fieldValue].Reset();
		buttonList[newIndex].Select();
		variantName.text = variantSelector.variants[newIndex].DisplayName;
		fieldValue = newIndex;
		SetFieldValue(fieldValue);
		return true;
	}
}
