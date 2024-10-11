using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenDeltaVStageInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI stageNumber;

	[SerializeField]
	private TextMeshProUGUI partCountNumber;

	[SerializeField]
	private TextMeshProUGUI partCountDecoupledNumber;

	[SerializeField]
	private TextMeshProUGUI startMassNumber;

	[SerializeField]
	private TextMeshProUGUI dryMassNumber;

	[SerializeField]
	private TextMeshProUGUI fuelMassNumber;

	[SerializeField]
	private TextMeshProUGUI endMassNumber;

	[SerializeField]
	private TextMeshProUGUI ispNumber;

	[SerializeField]
	private TextMeshProUGUI thrustNumber;

	[SerializeField]
	private TextMeshProUGUI twrNumber;

	[SerializeField]
	private TextMeshProUGUI deltavNumber;

	[SerializeField]
	private TextMeshProUGUI burnTimeNumber;

	[SerializeField]
	private TextMeshProUGUI payloadText;

	public DeltaVStageInfo stageInfo;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDeltaVStageInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateData(DeltaVStageInfo stageInfo, CalcType type)
	{
		throw null;
	}
}
