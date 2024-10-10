using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSelector : MonoBehaviour
{
	[SerializeField]
	public Button buttonPrevious;

	[SerializeField]
	public Button buttonNext;

	[SerializeField]
	public ScrollRect scrollMain;

	public int comboEntryIndex;

	public List<ComboButton> comboList;

	public int fieldValue;

	public Material previewBodyMaterial;

	public Material previewHelmetMaterial;

	public Material previewNeckringMaterial;

	public SuitButton suitButton;

	public ScrollRect ScrollMain => scrollMain;

	public void Awake()
	{
		comboList = new List<ComboButton>();
		buttonPrevious.onClick.AddListener(OnButtonPrev);
		buttonNext.onClick.AddListener(OnButtonNext);
	}

	public void OnDestroy()
	{
		buttonPrevious.onClick.RemoveListener(OnButtonPrev);
		buttonNext.onClick.RemoveListener(OnButtonNext);
	}

	public void OnButtonPrev()
	{
		int newIndex = (fieldValue + comboList.Count - 1) % comboList.Count;
		SelectVariant(newIndex);
	}

	public void OnButtonNext()
	{
		int newIndex = (fieldValue + 1) % comboList.Count;
		SelectVariant(newIndex);
	}

	public void SelectVariant(int newIndex)
	{
		GameEvents.onSuitComboSelection.Fire(this, comboList[newIndex].comboId, newIndex);
		fieldValue = newIndex;
	}
}
