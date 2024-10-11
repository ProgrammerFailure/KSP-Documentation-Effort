using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartSelectorEntry : EditorPartIcon
{
	public bool isSelected;

	protected Color selectionColor;

	protected MEPartSelectorBrowser partSelector;

	private Image image;

	protected AvailablePart part;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartSelectorEntry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(MEPartSelectorBrowser selector, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin, bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSelection(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartSelectorClick()
	{
		throw null;
	}
}
