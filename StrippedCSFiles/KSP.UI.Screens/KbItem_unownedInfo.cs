using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbItem_unownedInfo : KbItem
{
	public UIStateRawImage image;

	public Slider slider;

	public TextMeshProUGUI txtTrackingStatus;

	public TextMeshProUGUI txtSizeClass;

	public TextMeshProUGUI txtSizeClassValue;

	public TextMeshProUGUI txtSignalStrength;

	public TextMeshProUGUI txtLastSeen;

	public TextMeshProUGUI txtLastSeenValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_unownedInfo()
	{
		throw null;
	}
}
