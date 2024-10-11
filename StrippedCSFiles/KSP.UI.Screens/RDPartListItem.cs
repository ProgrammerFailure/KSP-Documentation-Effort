using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class RDPartListItem : EditorPartIcon
{
	public UIStateButton button;

	public TextMeshProUGUI label;

	public Image searchHighlight;

	public AvailablePart myPart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDPartListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string label, string buttonState, AvailablePart availablePart, PartUpgradeHandler.Upgrade upgrade)
	{
		throw null;
	}
}
