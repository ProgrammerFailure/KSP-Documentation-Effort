using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenDeltaVPropellantInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI propellantName;

	[SerializeField]
	private TextMeshProUGUI amountNumber;

	[SerializeField]
	private TextMeshProUGUI engineBurnsNumber;

	public DeltaVPropellantInfo propellantInfo;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDeltaVPropellantInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateData(DeltaVPropellantInfo propellantInfo, CalcType type)
	{
		throw null;
	}
}
