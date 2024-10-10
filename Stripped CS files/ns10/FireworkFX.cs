using UnityEngine;

namespace ns10;

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

	public float dopplerIntensity = 0.01f;

	public FireworkFXComponent fwBurst;

	public FireworkFXComponent fwTrail;

	public bool trailDestroyed;

	public bool shellCrashed;

	public AudioSource audioPlayer;

	public Rigidbody shellRB;

	public float targetPitch;

	public float currentPitch = 1f;

	public const float speedOfSound = 343.2f;

	public Vector3 startPosition = Vector3.zero;

	public void Setup(float fDuration, GameObject trailGO, GameObject burst, Color burstColor1, Color burstColor2, Color trailColor1, Color burstColor3, Color trailColor2, float tVelocity, float bSpread, float bDuration, float bFlareSize, string crackleSFXPath, bool randomizeBurstOrientation)
	{
		Setup(fDuration, trailGO, burst, burstColor1, burstColor2, trailColor1, burstColor3, trailColor2, tVelocity, bSpread, bDuration, bFlareSize, crackleSFXPath, randomizeBurstOrientation, -1f, -1f);
	}

	public void Setup(float fDuration, GameObject trailGO, GameObject burst, Color burstColor1, Color burstColor2, Color trailColor1, Color burstColor3, Color trailColor2, float tVelocity, float bSpread, float bDuration, float bFlareSize, string crackleSFXPath, bool randomizeBurstOrientation, float minTrailLifeTime, float maxTrailLifeTime)
	{
		this.burstColor1 = burstColor1;
		this.burstColor2 = burstColor2;
		this.trailColor1 = trailColor1;
		this.burstColor3 = burstColor3;
		this.trailColor2 = trailColor2;
		trailVelocity = tVelocity;
		burstSpread = bSpread;
		burstDuration = bDuration;
		burstFlareSize = bFlareSize;
		this.randomizeBurstOrientation = randomizeBurstOrientation;
		this.minTrailLifeTime = minTrailLifeTime;
		this.maxTrailLifeTime = maxTrailLifeTime;
		audioPlayer = GetComponent<AudioSource>();
		audioPlayer.loop = true;
		audioPlayer.clip = whistleSound;
		audioPlayer.spread = 180f;
		audioPlayer.spatialBlend = 1f;
		shellRB = GetComponent<Rigidbody>();
		flightTime = 0f;
		flightDuration = fDuration;
		burstInstance = Object.Instantiate(burst, Vector3.zero, Quaternion.identity);
		trailInstance = Object.Instantiate(trailGO, Vector3.zero, Quaternion.identity);
		if (burstInstance.GetComponent<FireworkFXComponent>() == null)
		{
			fwBurst = burstInstance.AddComponent<FireworkFXComponent>();
		}
		else
		{
			fwBurst = burstInstance.GetComponent<FireworkFXComponent>();
		}
		if (trailInstance.GetComponent<FireworkFXComponent>() == null)
		{
			fwTrail = trailInstance.AddComponent<FireworkFXComponent>();
		}
		else
		{
			fwTrail = trailInstance.GetComponent<FireworkFXComponent>();
		}
		fwTrail.Initialize(trailInstance, this, FireworkEffectType.TRAIL);
		fwBurst.Initialize(burstInstance, this, FireworkEffectType.BURST);
		fwBurst.audioSource.clip = explosionSound;
		if (!crackleSFXPath.Equals("none"))
		{
			fwBurst.crackleSFX = GameDatabase.Instance.GetAudioClip(crackleSFXPath);
		}
		trailDestroyed = false;
		startPosition = shellRB.transform.position;
	}

	public Vector3 GetRBVelocity()
	{
		if (!(shellRB == null))
		{
			return shellRB.velocity;
		}
		return Vector3.zero;
	}

	public void FixedUpdate()
	{
		flightTime += Time.fixedDeltaTime;
		if (flightTime > flightDuration)
		{
			fwBurst.ActivateBurstPS(base.transform.position);
			fwTrail.ActivateBurstPS(Vector3.zero);
			if (!trailDestroyed)
			{
				fwTrail.transform.SetParent(fwBurst.transform, worldPositionStays: true);
				trailDestroyed = true;
				Object.DestroyImmediate(base.gameObject);
			}
		}
		if (audioPlayer != null)
		{
			if (shellRB == null)
			{
				targetPitch = 0.5f;
			}
			else
			{
				float num = audioPlayer.maxDistance * 0.3f;
				targetPitch = Mathf.Max(343.2f / (343.2f + GetRBVelocity().magnitude) - (shellRB.transform.position - startPosition).magnitude * dopplerIntensity / num, 0.5f);
			}
			if (!audioPlayer.isPlaying && !trailDestroyed && !shellCrashed)
			{
				playAudioFX();
			}
			currentPitch = Mathf.Lerp(1f, targetPitch, flightTime / flightDuration);
			audioPlayer.pitch = currentPitch;
		}
	}

	[ContextMenu("Play Audio FX")]
	public void playAudioFX()
	{
		if (audioPlayer != null)
		{
			audioPlayer.Play();
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (audioPlayer != null && audioPlayer.isPlaying)
		{
			audioPlayer.Stop();
			shellCrashed = true;
		}
	}
}
