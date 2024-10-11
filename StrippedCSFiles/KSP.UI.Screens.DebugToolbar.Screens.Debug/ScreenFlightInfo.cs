using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenFlightInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI uT_text;

	[SerializeField]
	private TextMeshProUGUI physicsTimeRatio_text;

	[SerializeField]
	private TextMeshProUGUI latitude_txt;

	[SerializeField]
	private TextMeshProUGUI longitude_text;

	[SerializeField]
	private TextMeshProUGUI altitude_text;

	[SerializeField]
	private TextMeshProUGUI radar_altitude_text;

	[SerializeField]
	private TextMeshProUGUI biomeName_text;

	private Vessel currentActiveVessel;

	[SerializeField]
	private TextMeshProUGUI activeVesselName_text;

	[SerializeField]
	private TextMeshProUGUI refBody_text;

	[SerializeField]
	private TextMeshProUGUI frameOfReference_text;

	[SerializeField]
	private GameObject[] EVA_parameters;

	[SerializeField]
	private TextMeshProUGUI slopeAngle_text;

	[SerializeField]
	private float UpdateUISeconds;

	private float lastRealUITime;

	private bool gamePaused;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenFlightInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetActiveVesselName(Vessel data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameUnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUT(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLocation(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPhysics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVessel(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVesselSlopeAngle(bool active)
	{
		throw null;
	}
}
