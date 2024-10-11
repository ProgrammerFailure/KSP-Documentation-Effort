using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabOrbitAdv : ManeuverNodeEditorTab
{
	[SerializeField]
	private TextMeshProUGUI orbitArgumentOfPeriapsis;

	[SerializeField]
	private TextMeshProUGUI orbitLongitudeOfAscendingNode;

	[SerializeField]
	private TextMeshProUGUI ejectionAngle;

	[SerializeField]
	private TextMeshProUGUI orbitEccentricity;

	[SerializeField]
	private TextMeshProUGUI orbitInclination;

	private Orbit orbitToDisplay;

	private bool patchesAheadLimitOK;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabOrbitAdv()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsTabInteractable()
	{
		throw null;
	}
}
