using System;
using ns11;
using ns5;
using ns9;
using UnityEngine;

namespace ns24;

[Serializable]
public abstract class PartCategory
{
	public string name;

	public string displayName;

	public Icon icon;

	public string iconUrl;

	public Color color;

	public Color iconColor;

	public PartCategory()
	{
	}

	public virtual PartCategorizer.Category GetCategory()
	{
		EditorPartListFilter<AvailablePart> exclusionFilter = new EditorPartListFilter<AvailablePart>("Category_" + name, ExclusionCriteria);
		Icon icon = PartCategorizer.Instance.iconLoader.GetIcon(iconUrl);
		string text = displayName;
		return new PartCategorizer.Category(categorydisplayName: (!(text != "")) ? name : Localizer.Format(text), buttonType: PartCategorizer.ButtonType.SUBCATEGORY, displayType: EditorPartList.State.PartsList, categoryName: name, icon: icon, color: color, colorIcon: iconColor, exclusionFilter: exclusionFilter);
	}

	public abstract bool ExclusionCriteria(AvailablePart aP);
}
