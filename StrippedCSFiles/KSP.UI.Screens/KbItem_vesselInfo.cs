using System.Runtime.CompilerServices;
using TMPro;

namespace KSP.UI.Screens;

public class KbItem_vesselInfo : KbItem
{
	public UIStateRawImage image;

	public TextMeshProUGUI txtType;

	public TextMeshProUGUI txtPartcount;

	public TextMeshProUGUI txtPartcountValue;

	public TextMeshProUGUI txtMass;

	public TextMeshProUGUI txtMassValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_vesselInfo()
	{
		throw null;
	}
}
