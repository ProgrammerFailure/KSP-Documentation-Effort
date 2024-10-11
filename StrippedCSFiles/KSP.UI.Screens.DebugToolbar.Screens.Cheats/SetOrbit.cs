using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class SetOrbit : MonoBehaviour
{
	public Button bodyBackButton;

	public TextMeshProUGUI bodyNameText;

	public Button bodyForwardButton;

	public DebugScreenInputDouble eccInput;

	public DebugScreenInputDouble incInput;

	public DebugScreenInputDouble smaInput;

	public DebugScreenInputDouble mnaInput;

	public DebugScreenInputDouble lanInput;

	public DebugScreenInputDouble lpeInput;

	public DebugScreenInputDouble obtInput;

	public Button setOrbitButton;

	public TextMeshProUGUI errorText;

	public Button vesselBackButton;

	public TextMeshProUGUI vesselNameText;

	public Button vesselForwardButton;

	public DebugScreenInputDouble distanceInput;

	public Button rendezvousButton;

	public Toggle overrideSafetyCheck;

	private int selectedBody;

	private int selectedVessel;

	public static double safetyEnvelope;

	public static float rendezvousDistance;

	public CelestialBody SelectedBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel SelectedVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SetOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SetOrbit()
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
	private void OnSetOrbitClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckForErrors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRendezvousClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyBackClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyForwardClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSelectedBodyText()
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
	private void SetSelectedVesselText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetErrorMsg(string msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
