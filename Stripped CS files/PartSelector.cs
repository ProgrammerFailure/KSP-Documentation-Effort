using UnityEngine;

public class PartSelector : MonoBehaviour
{
	public Part lastHoveredPart;

	public Part part;

	public Callback<Part> onSelect;

	public bool hover;

	public Color edgeHighlightColor;

	public Color edgeHoverColor;

	public Color highlightColor;

	public Color hoverColor;

	public Part Host => part;

	public static PartSelector Create(Part host, Callback<Part> onSelect, Color highlightColor, Color hoverColor)
	{
		PartSelector partSelector = host.gameObject.AddComponent<PartSelector>();
		partSelector.part = host;
		partSelector.onSelect = onSelect;
		partSelector.highlightColor = highlightColor;
		partSelector.hoverColor = hoverColor;
		partSelector.edgeHighlightColor = highlightColor;
		partSelector.edgeHoverColor = hoverColor;
		partSelector.Setup();
		return partSelector;
	}

	public static PartSelector Create(Part host, Callback<Part> onSelect, Color highlightColor, Color hoverColor, Color edgeHighlightColor, Color edgeHoverColor)
	{
		PartSelector partSelector = host.gameObject.AddComponent<PartSelector>();
		partSelector.part = host;
		partSelector.onSelect = onSelect;
		partSelector.highlightColor = highlightColor;
		partSelector.hoverColor = hoverColor;
		partSelector.edgeHighlightColor = edgeHighlightColor;
		partSelector.edgeHoverColor = edgeHoverColor;
		partSelector.Setup();
		return partSelector;
	}

	public void Dismiss()
	{
		Object.Destroy(this);
	}

	public void Setup()
	{
		part.SetHighlightDefault();
		part.SetHighlightType(Part.HighlightType.AlwaysOn);
		part.SetHighlight(active: true, recursive: false);
		part.SetHighlightColor(highlightColor);
	}

	public void OnMouseEntered()
	{
		hover = true;
		part.SetHighlightColor(hoverColor);
	}

	public void LateUpdate()
	{
		if (lastHoveredPart != part && Mouse.HoveredPart == part)
		{
			OnMouseEntered();
		}
		else if (lastHoveredPart == part && Mouse.HoveredPart != part)
		{
			OnMouseExited();
		}
		lastHoveredPart = Mouse.HoveredPart;
		if (hover && ((Mouse.Left.GetClick() && !Mouse.Left.WasDragging()) || (Mouse.Right.GetClick() && !Mouse.Right.WasDragging())))
		{
			onSelect(part);
		}
	}

	public void OnMouseExited()
	{
		hover = false;
		part.SetHighlightColor(highlightColor);
	}

	public void OnDestroy()
	{
		if (part != null)
		{
			part.SetHighlightDefault();
		}
	}
}
