using System.Runtime.CompilerServices;
using UnityEngine;

public class AmbienceControl : MonoBehaviour
{
	public AudioSource editorAmbience;

	public AudioSource flightAmbienceHigh;

	public AudioSource flightAmbienceLow;

	public AudioSource airspeed;

	public AudioSource gForceNoise;

	public AnimationCurve flightLowAltitudeCurve;

	public AnimationCurve flightHighAltitudeCurve;

	public GameObject cameraRef;

	private float stPatm;

	private float camAlt;

	public float ambienceGain;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AmbienceControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeavingScene(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void editorAmbienceGo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void editorAmbienceStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void flightAmbienceGo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void flightAmbienceStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
