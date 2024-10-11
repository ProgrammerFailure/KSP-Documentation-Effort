using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using RUI.Icons.Selectable;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MENodeCategoryButton : MonoBehaviour
{
	public RawImage buttonImage;

	public Button categoryButton;

	public string categoryName;

	public string categoryDisplayName;

	public TooltipController_Text tooltipController;

	public Sprite normalState;

	public Sprite selectedState;

	[SerializeField]
	private Image highlighter;

	private MENodeCategorizer nodeCategorizer;

	private Icon icon;

	private Color colorWhite;

	private Color colorBlack;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodeCategoryButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string newCategoryName, string newCategoryDisplayName, MENodeCategorizer newNodeCategorizer, Icon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeselectCategory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectCategory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetIcon(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHighlighter(bool state)
	{
		throw null;
	}
}
