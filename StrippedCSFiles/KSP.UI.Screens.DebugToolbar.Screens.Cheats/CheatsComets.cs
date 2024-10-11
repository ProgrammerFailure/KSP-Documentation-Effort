using System.Runtime.CompilerServices;
using SentinelMission;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class CheatsComets : MonoBehaviour
{
	public Button KillButton;

	public Button genRandomButton;

	public Button genShortButton;

	public Button genMidButton;

	public Button genLongButton;

	public Button genInterstellarButton;

	public Button sentinelBackButton;

	public TextMeshProUGUI sentinelNameText;

	public Button sentinelForwardButton;

	public Button genSentinelButton;

	public Button genSentinelShortButton;

	public Button genSentinelMidButton;

	public Button genSentinelLongButton;

	public Button genSentinelInterButton;

	public Toggle collidersEnabled;

	public Toggle geysersEnabled;

	public Toggle dustsEnabled;

	public Toggle comaEnabled;

	public Toggle ionTailEnabled;

	public Toggle dustTailEnabled;

	public Slider geyserEmitterMultiplier;

	public TextMeshProUGUI geyserMultiplierText;

	public Slider snowEmitterMultiplier;

	public TextMeshProUGUI snowMultiplierText;

	public Slider ionEmitterMultiplier;

	public TextMeshProUGUI ionMultiplierText;

	public Slider dustEmitterMultiplier;

	public TextMeshProUGUI dustMultiplierText;

	public Slider dustSizeMultiplier;

	public TextMeshProUGUI dustSizeMultiplierText;

	public Button rePrimeParticles;

	private int selectedSentinel;

	private SentinelScenario sentinelScenario;

	private float gametime;

	public Vessel SelectedSentinel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheatsComets()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSentinelOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKillClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRandomClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnShortClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMidClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLongClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInterstellarClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSentinelClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSentinelShortClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSentinelMidClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSentinelLongClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSentinelInterstellarClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselBackClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselForwardClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSelectedSentinelText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCollidersClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGeysersClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDustClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnComaClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIonTailClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDustTailClick(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGeyserMultiplierChange(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSnowMultiplierChange(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDustMultiplierChange(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIonMultiplierChange(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDustSizeMultiplierChange(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnReprimeParticles()
	{
		throw null;
	}
}
