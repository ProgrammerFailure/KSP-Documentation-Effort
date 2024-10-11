using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbItem_kerbalInfo : KbItem
{
	public UIStateImage xpStars;

	public RawImage kerbalIcon;

	public TextMeshProUGUI txtSeat;

	public TextMeshProUGUI txtName;

	public TextMeshProUGUI txtXp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_kerbalInfo()
	{
		throw null;
	}
}
