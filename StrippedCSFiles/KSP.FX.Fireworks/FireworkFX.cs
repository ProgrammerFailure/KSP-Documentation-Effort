using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.FX.Fireworks;

public class FireworkFX : MonoBehaviour
{
	public float flightDuration;

	public float flightTime;

	public GameObject burstInstance;

	public GameObject trailInstance;

	public Color burstColor1;

	public Color burstColor2;

	public Color burstColor3;

	public Color trailColor1;

	public Color trailColor2;

	public float trailVelocity;

	public float burstSpread;

	public float burstDuration;

	public float burstFlareSize;

	public bool randomizeBurstOrientation;

	public float minTrailLifeTime;

	public float maxTrailLifeTime;

	public AudioClip whistleSound;

	public AudioClip explosionSound;

	public float dopplerIntensity;

	private FireworkFXComponent fwBurst;

	private FireworkFXComponent fwTrail;

	private bool trailDestroyed;

	private bool shellCrashed;

	private AudioSource audioPlayer;

	private Rigidbody shellRB;

	private float targetPitch;

	private float currentPitch;

	private const float speedOfSound = 343.2f;

	private Vector3 startPosition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FireworkFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(float fDuration, GameObject trailGO, GameObject burst, Color burstColor1, Color burstColor2, Color trailColor1, Color burstColor3, Color trailColor2, float tVelocity, float bSpread, float bDuration, float bFlareSize, string crackleSFXPath, bool randomizeBurstOrientation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(float fDuration, GameObject trailGO, GameObject burst, Color burstColor1, Color burstColor2, Color trailColor1, Color burstColor3, Color trailColor2, float tVelocity, float bSpread, float bDuration, float bFlareSize, string crackleSFXPath, bool randomizeBurstOrientation, float minTrailLifeTime, float maxTrailLifeTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetRBVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Play Audio FX")]
	private void playAudioFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCollisionEnter(Collision collision)
	{
		throw null;
	}
}
