using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class AltitudeTumbler : MonoBehaviour
{
	internal class UsageStats
	{
		public int aglSelected;

		public int aslSelected;

		internal bool UsageFound
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UsageStats()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void Reset()
		{
			throw null;
		}
	}

	public static AltitudeTumbler Instance;

	public Tumbler tumbler;

	public static double altitudeMultiplier;

	public Button altitudeModeBtn;

	private AltimeterDisplayState aglMode;

	public SpriteTumblerObject modeTumbler;

	public float modeTumblerSharpness;

	private float lastUpdateTime;

	internal UsageStats usage;

	public AltimeterDisplayState CurrentMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AltitudeTumbler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AltitudeTumbler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTumblerPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeAltimeterMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetModeTumbler(AltimeterDisplayState mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapEntered()
	{
		throw null;
	}
}
