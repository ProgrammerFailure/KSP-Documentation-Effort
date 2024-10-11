using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionsAppVesselInfo
{
	public TextMeshProUGUI vesselName;

	public Button headerButton;

	public UIStateButton editorButton;

	public TooltipController_Text tooltipeditorButton;

	public UIStateButton stateButton;

	public TooltipController_Text tooltipstateButton;

	public Image readyImage;

	public VesselSituation vesselSituation;

	public Mission mission;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsAppVesselInfo()
	{
		throw null;
	}
}
