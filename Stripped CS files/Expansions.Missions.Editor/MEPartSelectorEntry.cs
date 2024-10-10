using ns11;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartSelectorEntry : EditorPartIcon
{
	public bool isSelected;

	public Color selectionColor;

	public MEPartSelectorBrowser partSelector;

	public Image image;

	public AvailablePart part;

	public void Create(MEPartSelectorBrowser selector, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin, bool selected)
	{
		Create(null, part, iconSize, iconOverScale, iconOverSpin);
		image = base.gameObject.GetComponent<Image>();
		this.part = part;
		partSelector = selector;
		UpdateSelection(selected);
		GetComponent<Button>().onClick.AddListener(OnPartSelectorClick);
	}

	public void UpdateSelection(bool selected)
	{
		if (isSelected != selected)
		{
			partSelector.PartCategorizer.PartSelected(part, selected);
		}
		isSelected = selected;
		selectionColor = partSelector.SelectionColor;
		image.color = (isSelected ? selectionColor : Color.white);
	}

	public void OnPartSelectorClick()
	{
		UpdateSelection(!isSelected);
		partSelector.OnPartSelectionChange(base.partInfo, isSelected);
	}
}
