using System.Collections.Generic;
using Highlighting;
using UnityEngine;

namespace ns11;

public class EditorActionPartSelector : MonoBehaviour
{
	public Part part;

	public bool isSelected;

	public bool isHovered;

	public bool isHighlighted;

	public static void CreatePartActions(List<Part> ship)
	{
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			CreatePartActions(ship[i]);
		}
	}

	public static EditorActionPartSelector CreatePartActions(Part p)
	{
		EditorActionPartSelector editorActionPartSelector = p.GetComponent<EditorActionPartSelector>();
		if (editorActionPartSelector == null)
		{
			editorActionPartSelector = p.gameObject.AddComponent<EditorActionPartSelector>();
		}
		editorActionPartSelector.part = p;
		p.SetHighlightType(Part.HighlightType.Disabled);
		return editorActionPartSelector;
	}

	public static void DestroyPartActions(List<Part> ship)
	{
		int count = ship.Count;
		while (count-- > 0)
		{
			DestroyPartActions(ship[count]);
		}
	}

	public static void DestroyPartActions(Part p)
	{
		EditorActionPartSelector component = p.GetComponent<EditorActionPartSelector>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
		p.SetHighlightDefault();
	}

	public void LateUpdate()
	{
		if (part == null)
		{
			return;
		}
		bool flag = part == Mouse.HoveredPart;
		bool flag2 = false;
		if (HighLogic.LoadedSceneIsEditor)
		{
			flag2 = EditorPanels.Instance != null && EditorPanels.Instance.IsMouseOver();
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			flag2 = EditorActionGroups.Instance != null && EditorActionGroups.Instance.isMouseOver;
		}
		if (flag != isHovered)
		{
			isHovered = flag;
			if (!isSelected)
			{
				if (isHovered && !flag2)
				{
					part.SetHighlight(active: true, recursive: false);
				}
				if (!isHovered)
				{
					part.SetHighlight(active: false, recursive: false);
				}
			}
		}
		if (isHovered && Mouse.Left.GetButtonDown() && !flag2)
		{
			Select();
		}
	}

	public void Select()
	{
		if (!GameSettings.MODIFIER_KEY.GetKey())
		{
			EditorActionGroups.Instance.ClearSelection(reconstruct: false);
		}
		if (part.FindModuleImplementing<KerbalEVA>() != null)
		{
			return;
		}
		EditorActionGroups.Instance.AddToSelection(this);
		part.SetHighlightType(Part.HighlightType.AlwaysOn);
		part.SetHighlightColor(Highlighter.colorPartEditorActionSelected);
		part.SetHighlight(active: true, recursive: false);
		isSelected = true;
		if (part.partInfo != null && !part.partInfo.mapActionsToSymmetryParts)
		{
			return;
		}
		int count = part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			Part obj = part.symmetryCounterparts[count];
			obj.SetHighlightType(Part.HighlightType.AlwaysOn);
			obj.SetHighlightColor(Highlighter.colorPartEditorActionSelected);
			obj.SetHighlight(active: true, recursive: false);
			EditorActionPartSelector component = obj.gameObject.GetComponent<EditorActionPartSelector>();
			if (component != null)
			{
				component.isSelected = true;
			}
		}
	}

	public void Deselect()
	{
		part.SetHighlightType(Part.HighlightType.Disabled);
		part.SetHighlightColor();
		part.SetHighlight(active: false, recursive: false);
		isSelected = false;
		if (part.partInfo != null && !part.partInfo.mapActionsToSymmetryParts)
		{
			return;
		}
		int count = part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			Part obj = part.symmetryCounterparts[count];
			obj.SetHighlightType(Part.HighlightType.Disabled);
			obj.SetHighlightColor();
			obj.SetHighlight(active: false, recursive: false);
			EditorActionPartSelector component = obj.gameObject.GetComponent<EditorActionPartSelector>();
			if (component != null)
			{
				component.isSelected = false;
			}
		}
	}

	public void HighLight(bool highLight)
	{
		if (highLight && !isHighlighted)
		{
			isHighlighted = true;
			part.SetHighlightType(Part.HighlightType.AlwaysOn);
			part.SetHighlightColor(Highlighter.colorPartEditorActionHighlight);
			part.SetHighlight(active: true, recursive: false);
			if (part.partInfo == null || part.partInfo.mapActionsToSymmetryParts)
			{
				int count = part.symmetryCounterparts.Count;
				while (count-- > 0)
				{
					Part obj = part.symmetryCounterparts[count];
					obj.SetHighlightType(Part.HighlightType.AlwaysOn);
					obj.SetHighlightColor(Highlighter.colorPartEditorActionHighlight);
					obj.SetHighlight(active: true, recursive: false);
				}
			}
		}
		else
		{
			if (highLight || !isHighlighted)
			{
				return;
			}
			isHighlighted = false;
			if (isSelected)
			{
				part.SetHighlightType(Part.HighlightType.AlwaysOn);
				part.SetHighlightColor(Highlighter.colorPartEditorActionSelected);
				part.SetHighlight(active: true, recursive: false);
				if (part.partInfo == null || part.partInfo.mapActionsToSymmetryParts)
				{
					int count2 = part.symmetryCounterparts.Count;
					while (count2-- > 0)
					{
						Part obj2 = part.symmetryCounterparts[count2];
						obj2.SetHighlightType(Part.HighlightType.AlwaysOn);
						obj2.SetHighlightColor(Highlighter.colorPartEditorActionSelected);
						obj2.SetHighlight(active: true, recursive: false);
					}
				}
				return;
			}
			part.SetHighlightType(Part.HighlightType.Disabled);
			part.SetHighlightColor();
			part.SetHighlight(active: false, recursive: false);
			if (part.partInfo == null || part.partInfo.mapActionsToSymmetryParts)
			{
				int count3 = part.symmetryCounterparts.Count;
				while (count3-- > 0)
				{
					Part obj3 = part.symmetryCounterparts[count3];
					obj3.SetHighlightType(Part.HighlightType.Disabled);
					obj3.SetHighlightColor();
					obj3.SetHighlight(active: false, recursive: false);
				}
			}
		}
	}
}
