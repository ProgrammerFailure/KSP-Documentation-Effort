using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionsAppObjectiveInfo
{
	public TextMeshProUGUI objectiveName;

	public TextMeshProUGUI status;

	public Button headerButton;

	public TooltipController_Text tooltipHeader;

	public Image readyImage;

	public MENode node;

	public Mission mission;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsAppObjectiveInfo()
	{
		throw null;
	}
}
