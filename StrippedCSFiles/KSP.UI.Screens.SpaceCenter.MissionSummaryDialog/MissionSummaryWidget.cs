using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

public class MissionSummaryWidget : MonoBehaviour
{
	private RectTransform rTrf;

	protected MissionRecoveryDialog host;

	[SerializeField]
	protected TextMeshProUGUI header;

	public RectTransform RTrf
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionSummaryWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Init(MissionRecoveryDialog host)
	{
		throw null;
	}
}
