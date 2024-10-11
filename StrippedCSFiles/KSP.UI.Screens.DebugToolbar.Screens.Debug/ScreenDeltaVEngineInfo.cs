using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenDeltaVEngineInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI engineName;

	[SerializeField]
	private TextMeshProUGUI engineType;

	[SerializeField]
	private TextMeshProUGUI maxThrustNumber;

	[SerializeField]
	private TextMeshProUGUI maxThrustType;

	[SerializeField]
	private TextMeshProUGUI ispNumber;

	[SerializeField]
	private TextMeshProUGUI ispType;

	[SerializeField]
	private TextMeshProUGUI maxBurnTimeNumber;

	public DeltaVEngineInfo engineInfo;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDeltaVEngineInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateData(DeltaVEngineInfo engineInfo, CalcType type, string typeDesc, int stage)
	{
		throw null;
	}
}
