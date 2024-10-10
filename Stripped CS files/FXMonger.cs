using System;
using System.Collections;
using System.Collections.Generic;
using Expansions;
using ns10;
using UnityEngine;

public class FXMonger : MonoBehaviour
{
	public class ProtoExplosion
	{
		public Vector3d position;

		public double power;

		public List<Part> sources;

		public float timeOfExplosion;

		public float timeOfFX;

		public ProtoExplosion(Part source, Vector3d pos, double powah)
		{
			sources = new List<Part>();
			sources.Add(source);
			position = pos;
			power = powah;
			timeOfExplosion = Time.timeSinceLevelLoad;
			timeOfFX = 0f;
		}
	}

	public class ROCProtoExplosion
	{
		public Vector3d position;

		public double power;

		public ROCProtoExplosion(Vector3 pos, double powah)
		{
			position = pos;
			power = powah;
		}
	}

	public class DebrisProtoExplosion
	{
		public Vector3d position;

		public double power;

		public GameObject objectToFollow;

		public DebrisProtoExplosion(Vector3 pos, double powah, GameObject otf)
		{
			position = pos;
			power = powah;
			objectToFollow = otf;
		}
	}

	public List<GameObject> debrisExplosions = new List<GameObject>();

	public List<GameObject> debrisExplosionsObjectsToFollow = new List<GameObject>();

	public GameObject[] explosions;

	public AudioClip[] explosionSounds;

	public GameObject[] thuds;

	public AudioClip[] crashSounds;

	public GameObject[] splashes;

	public AudioClip[] splashSounds;

	public GameObject[] debrisExplosion;

	public AudioClip[] debrisSounds;

	public float minSqrBlast;

	public float minSqrSplash;

	public float minDistanceFromOtherBlasts = 10f;

	public float minDistanceFromOtherSplashes = 1.5f;

	public float minTimeBetweenSplashes = 0.025f;

	public float queueRemovalTimeMultiplier = 5f;

	public double minPower = 0.001;

	public List<ProtoExplosion> queuedExplosions = new List<ProtoExplosion>();

	public List<ROCProtoExplosion> queuedROCExplosions = new List<ROCProtoExplosion>();

	public List<DebrisProtoExplosion> queuedDebrisExplosions = new List<DebrisProtoExplosion>();

	public List<FXObject> explosionObjects = new List<FXObject>();

	public static FXMonger fetch;

	public bool splashMonger = true;

	public List<ProtoExplosion> queuedSplashes = new List<ProtoExplosion>();

	public static ParticleSystem.Particle[] sParts = new ParticleSystem.Particle[1];

	public List<FireworkFXComponent> fireworks = new List<FireworkFXComponent>();

	public static ParticleSystem.Particle[] fireworkPSs = new ParticleSystem.Particle[1];

	[Obsolete("No longer used - please use minDistanceFromOtherBlasts")]
	public float MINIMUM_DISTANCE_FROM_OTHER_BLASTS => 10f;

	public void Awake()
	{
		fetch = this;
		minSqrBlast = minDistanceFromOtherBlasts;
		minSqrBlast *= minSqrBlast;
		minTimeBetweenSplashes = GameSettings.MIN_TIME_BETWEEN_SPLASHES;
		minDistanceFromOtherSplashes = GameSettings.MIN_DISTANCE_FROM_OTHER_SPLASHES;
		minSqrSplash = minDistanceFromOtherSplashes;
		minSqrSplash *= minSqrSplash;
		if (minPower <= 0.0)
		{
			minPower = 0.001;
		}
		else if (minPower > 1.0)
		{
			minPower = 1.0;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public static void Explode(Part source, Vector3d blastPos, double howhard)
	{
		if (fetch != null)
		{
			fetch.explode(source, blastPos, howhard);
		}
	}

	public void explode(Part source, Vector3d blastPos, double howHard)
	{
		if (howHard < minPower)
		{
			howHard = minPower;
		}
		int count = queuedExplosions.Count;
		int num = 0;
		ProtoExplosion protoExplosion;
		while (true)
		{
			if (num < count)
			{
				protoExplosion = queuedExplosions[num];
				if (!((protoExplosion.position - blastPos).sqrMagnitude >= (double)minSqrBlast))
				{
					break;
				}
				num++;
				continue;
			}
			queuedExplosions.Add(new ProtoExplosion(source, blastPos, howHard));
			return;
		}
		double num2 = protoExplosion.power + howHard;
		if (num2 != 0.0)
		{
			protoExplosion.position = (protoExplosion.position * protoExplosion.power + blastPos * howHard) / num2;
		}
		else
		{
			protoExplosion.position = (protoExplosion.position + blastPos) / 2.0;
		}
		protoExplosion.sources.Add(source);
		if (GameSettings.LOG_FXMONGER_VERBOSE)
		{
			PDebug.Log("[Explosion] Combined.");
		}
	}

	public static void ROCExplode(Vector3d blastPos, double howhard)
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && fetch != null)
		{
			fetch.rocexplode(blastPos, howhard);
		}
	}

	public void rocexplode(Vector3d blastPos, double howHard)
	{
		if (howHard < minPower)
		{
			howHard = minPower;
		}
		int count = queuedROCExplosions.Count;
		int num = 0;
		ROCProtoExplosion rOCProtoExplosion;
		while (true)
		{
			if (num < count)
			{
				rOCProtoExplosion = queuedROCExplosions[num];
				if (!((rOCProtoExplosion.position - blastPos).sqrMagnitude >= (double)minSqrBlast))
				{
					break;
				}
				num++;
				continue;
			}
			queuedROCExplosions.Add(new ROCProtoExplosion(blastPos, howHard));
			return;
		}
		double num2 = rOCProtoExplosion.power + howHard;
		if (num2 != 0.0)
		{
			rOCProtoExplosion.position = (rOCProtoExplosion.position * rOCProtoExplosion.power + blastPos * howHard) / num2;
		}
		else
		{
			rOCProtoExplosion.position = (rOCProtoExplosion.position + blastPos) / 2.0;
		}
		if (GameSettings.LOG_FXMONGER_VERBOSE)
		{
			PDebug.Log("[ROCExplosion] Combined.");
		}
	}

	public static void ExplodeWithDebris(Vector3d blastPos, double howHard, GameObject objectToFollow)
	{
		if (fetch != null)
		{
			fetch.SpawnDebrisExplosions(blastPos, howHard, objectToFollow);
		}
	}

	public void SpawnDebrisExplosions(Vector3d blastPos, double howHard, GameObject objectToFollow)
	{
		if (howHard < minPower)
		{
			howHard = minPower;
		}
		int count = queuedDebrisExplosions.Count;
		int num = 0;
		DebrisProtoExplosion debrisProtoExplosion;
		while (true)
		{
			if (num < count)
			{
				debrisProtoExplosion = queuedDebrisExplosions[num];
				if (!((debrisProtoExplosion.position - blastPos).sqrMagnitude >= (double)minSqrBlast))
				{
					break;
				}
				num++;
				continue;
			}
			queuedDebrisExplosions.Add(new DebrisProtoExplosion(blastPos, howHard, objectToFollow));
			return;
		}
		double num2 = debrisProtoExplosion.power + howHard;
		debrisProtoExplosion.position = (debrisProtoExplosion.position * debrisProtoExplosion.power + blastPos * howHard) / num2;
		PDebug.Log("[ROCExplosion] Combined.");
	}

	public void LateUpdate()
	{
		int num = 0;
		int count = queuedExplosions.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoExplosion protoExplosion = queuedExplosions[i];
			if (protoExplosion == null)
			{
				continue;
			}
			double num2 = UtilMath.Clamp01(protoExplosion.power);
			if (!double.IsNaN(protoExplosion.position.x) && !double.IsNaN(protoExplosion.position.y) && !double.IsNaN(protoExplosion.position.z))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(explosions[(int)(num2 * (double)(explosions.Length - 1))]);
				gameObject.SetActive(value: true);
				gameObject.transform.position = protoExplosion.position;
				gameObject.transform.up = FlightGlobals.upAxis;
				AudioClip effectSound = explosionSounds[(int)(num2 * (double)(explosionSounds.Length - 1))];
				FXObject fXObject = new FXObject(gameObject);
				fXObject.effectSound = effectSound;
				gameObject.AddComponent<FXObjectPhoneHome>().parent = fXObject;
				explosionObjects.Add(fXObject);
				if (!gameObject.GetComponent<AudioSource>())
				{
					gameObject.gameObject.AddComponent<AudioSource>();
				}
				gameObject.GetComponent<AudioSource>().PlayOneShot(fXObject.effectSound, GameSettings.SHIP_VOLUME);
				num++;
			}
		}
		if (num > 0 && GameSettings.LOG_FXMONGER_VERBOSE)
		{
			PDebug.Log(num + " explosions created.");
		}
		count = queuedSplashes.Count;
		for (int j = 0; j < count; j++)
		{
			ProtoExplosion protoExplosion = queuedSplashes[j];
			if (protoExplosion != null && !(protoExplosion.timeOfFX > 0f))
			{
				double num3 = UtilMath.Clamp01(protoExplosion.power);
				if (!double.IsNaN(protoExplosion.position.x) && !double.IsNaN(protoExplosion.position.y) && !double.IsNaN(protoExplosion.position.z))
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(splashes[(int)(num3 * (double)(splashes.Length - 1))]);
					gameObject2.gameObject.SetActive(value: true);
					gameObject2.transform.position = protoExplosion.position;
					gameObject2.transform.up = FlightGlobals.getUpAxis(FlightGlobals.currentMainBody, protoExplosion.position);
					protoExplosion.timeOfFX = Time.timeSinceLevelLoad;
					gameObject2.transform.Translate(0f, 0f - FlightGlobals.getAltitudeAtPos(gameObject2.transform.position, FlightGlobals.currentMainBody), 0f);
					AudioClip effectSound2 = splashSounds[(int)(num3 * (double)(splashSounds.Length - 1))];
					FXObject fXObject2 = new FXObject(gameObject2);
					fXObject2.effectSound = effectSound2;
					gameObject2.AddComponent<FXObjectPhoneHome>().parent = fXObject2;
					explosionObjects.Add(fXObject2);
					num++;
				}
			}
		}
		if (num > 0 && GameSettings.LOG_FXMONGER_VERBOSE)
		{
			PDebug.Log(num + " splashes created.");
		}
		int num4 = 0;
		int count2 = queuedROCExplosions.Count;
		for (int k = 0; k < count2; k++)
		{
			ROCProtoExplosion rOCProtoExplosion = queuedROCExplosions[k];
			if (rOCProtoExplosion == null)
			{
				continue;
			}
			double num5 = UtilMath.Clamp01(rOCProtoExplosion.power);
			if (!double.IsNaN(rOCProtoExplosion.position.x) && !double.IsNaN(rOCProtoExplosion.position.y) && !double.IsNaN(rOCProtoExplosion.position.z))
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(explosions[(int)(num5 * (double)(explosions.Length - 1))]);
				gameObject3.SetActive(value: true);
				gameObject3.transform.position = rOCProtoExplosion.position;
				gameObject3.transform.up = FlightGlobals.upAxis;
				AudioClip effectSound3 = explosionSounds[(int)(num5 * (double)(explosionSounds.Length - 1))];
				FXObject fXObject3 = new FXObject(gameObject3);
				fXObject3.effectSound = effectSound3;
				gameObject3.AddComponent<FXObjectPhoneHome>().parent = fXObject3;
				explosionObjects.Add(fXObject3);
				if (!gameObject3.GetComponent<AudioSource>())
				{
					gameObject3.gameObject.AddComponent<AudioSource>();
				}
				gameObject3.GetComponent<AudioSource>().PlayOneShot(fXObject3.effectSound, GameSettings.SHIP_VOLUME);
				num4++;
			}
		}
		if (num4 > 0 && GameSettings.LOG_FXMONGER_VERBOSE)
		{
			PDebug.Log(num4 + " ROC explosions created.");
		}
		int count3 = queuedSplashes.Count;
		while (count3-- > 0)
		{
			if (queuedSplashes[count3].timeOfExplosion + minTimeBetweenSplashes * queueRemovalTimeMultiplier < Time.timeSinceLevelLoad)
			{
				queuedSplashes.RemoveAt(count3);
			}
		}
		int num6 = 0;
		int count4 = queuedDebrisExplosions.Count;
		for (int l = 0; l < count4; l++)
		{
			DebrisProtoExplosion debrisProtoExplosion = queuedDebrisExplosions[l];
			if (debrisProtoExplosion != null)
			{
				double num7 = UtilMath.Clamp01(debrisProtoExplosion.power);
				GameObject gameObject4 = UnityEngine.Object.Instantiate(debrisExplosion[(int)(num7 * (double)(debrisExplosion.Length - 1))]);
				gameObject4.SetActive(value: true);
				gameObject4.transform.position = debrisProtoExplosion.position;
				gameObject4.transform.up = FlightGlobals.upAxis;
				AudioClip effectSound4 = debrisSounds[(int)(num7 * (double)(debrisSounds.Length - 1))];
				FXObject fXObject4 = new FXObject(gameObject4);
				fXObject4.effectSound = effectSound4;
				gameObject4.AddComponent<FXObjectPhoneHome>().parent = fXObject4;
				explosionObjects.Add(fXObject4);
				if (!gameObject4.GetComponent<AudioSource>())
				{
					gameObject4.gameObject.AddComponent<AudioSource>();
				}
				gameObject4.GetComponent<AudioSource>().PlayOneShot(fXObject4.effectSound, GameSettings.SHIP_VOLUME);
				if (debrisProtoExplosion.objectToFollow != null)
				{
					debrisExplosions.Add(gameObject4);
					debrisExplosionsObjectsToFollow.Add(debrisProtoExplosion.objectToFollow);
				}
				num6++;
			}
		}
		if (num6 > 0)
		{
			PDebug.Log(num6 + " Debris explosions created.");
		}
		for (int num8 = debrisExplosions.Count - 1; num8 >= 0; num8--)
		{
			if (debrisExplosions[num8] != null && debrisExplosionsObjectsToFollow[num8] != null)
			{
				debrisExplosions[num8].transform.position = debrisExplosionsObjectsToFollow[num8].transform.position;
			}
			else
			{
				debrisExplosions.RemoveAt(num8);
				debrisExplosionsObjectsToFollow.RemoveAt(num8);
			}
		}
		queuedExplosions.Clear();
		queuedROCExplosions.Clear();
		queuedDebrisExplosions.Clear();
	}

	public static FXObject Splash(Vector3 pos, float howHard)
	{
		if ((bool)fetch)
		{
			if (!fetch.splashMonger)
			{
				return fetch.splash(pos, howHard);
			}
			fetch.splashMongered(pos, howHard);
			return null;
		}
		return null;
	}

	public FXObject splash(Vector3 pos, float howHard)
	{
		howHard = Mathf.Clamp01(howHard);
		GameObject gameObject = UnityEngine.Object.Instantiate(splashes[(int)(howHard * (float)(splashes.Length - 1))]);
		gameObject.gameObject.SetActive(value: true);
		gameObject.transform.position = pos;
		gameObject.transform.up = FlightGlobals.getUpAxis(FlightGlobals.currentMainBody, pos);
		gameObject.transform.Translate(0f, 0f - FlightGlobals.getAltitudeAtPos(gameObject.transform.position, FlightGlobals.currentMainBody), 0f);
		AudioClip effectSound = splashSounds[(int)(howHard * (float)(splashSounds.Length - 1))];
		FXObject fXObject = new FXObject(gameObject);
		fXObject.effectSound = effectSound;
		gameObject.AddComponent<FXObjectPhoneHome>().parent = fXObject;
		explosionObjects.Add(fXObject);
		return fXObject;
	}

	public void splashMongered(Vector3 pos, float howHard)
	{
		howHard = Mathf.Clamp01(howHard);
		int count = queuedSplashes.Count;
		int num = 0;
		ProtoExplosion protoExplosion;
		while (true)
		{
			if (num < count)
			{
				protoExplosion = queuedSplashes[num];
				if (!((protoExplosion.position - pos).sqrMagnitude >= (double)minSqrSplash))
				{
					break;
				}
				num++;
				continue;
			}
			queuedSplashes.Add(new ProtoExplosion(null, pos, howHard));
			return;
		}
		if (!(protoExplosion.timeOfFX + minTimeBetweenSplashes < Time.timeSinceLevelLoad))
		{
			double num2 = protoExplosion.power + (double)howHard;
			if (num2 != 0.0)
			{
				protoExplosion.position = (protoExplosion.position * protoExplosion.power + pos * howHard) / num2;
			}
			else
			{
				protoExplosion.position = (protoExplosion.position + pos) / 2.0;
			}
			if (GameSettings.LOG_FXMONGER_VERBOSE)
			{
				PDebug.Log("[Splash] Combined.");
			}
		}
	}

	public IEnumerator explosionTest()
	{
		yield return new WaitForSeconds(5f);
		explode(null, Vector3.up * 5f, 1.0);
		MonoBehaviour.print("exploded.");
	}

	public void offsetPositions(Vector3d offset)
	{
		int count = explosionObjects.Count;
		while (count-- > 0)
		{
			FXObject fXObject = explosionObjects[count];
			GameObject effectObj = explosionObjects[count].effectObj;
			if (effectObj == null)
			{
				fXObject.effectObj = null;
				fXObject.effectSound = null;
				fXObject.systems.Clear();
				explosionObjects.RemoveAt(count);
			}
			effectObj.transform.position = (Vector3d)effectObj.transform.position + offset;
			int count2 = fXObject.systems.Count;
			while (count2-- > 0)
			{
				ParticleSystem particleSystem = fXObject.systems[count2];
				if (particleSystem == null)
				{
					continue;
				}
				ParticleSystem.MainModule main = particleSystem.main;
				if (main.simulationSpace == ParticleSystemSimulationSpace.World && particleSystem.particleCount > 0)
				{
					int maxParticles = main.maxParticles;
					if (sParts.Length < maxParticles)
					{
						sParts = new ParticleSystem.Particle[maxParticles];
					}
					int particles = particleSystem.GetParticles(sParts);
					int num = particles;
					while (num-- > 0)
					{
						sParts[num].position = (Vector3d)sParts[num].position + offset;
					}
					particleSystem.SetParticles(sParts, particles);
				}
			}
		}
	}

	public static void RemoveFXOjbect(FXObject obj)
	{
		obj.effectObj = null;
		obj.effectSound = null;
		obj.systems.Clear();
		if ((bool)fetch)
		{
			fetch.explosionObjects.Remove(obj);
		}
	}

	public static void OffsetPositions(Vector3d offset)
	{
		if ((bool)fetch)
		{
			fetch.offsetPositions(offset);
			fetch.offsetFireworks(offset);
		}
	}

	public static void AddFireworkFX(FireworkFXComponent fx)
	{
		if ((bool)fetch)
		{
			fetch.fireworks.Add(fx);
		}
	}

	public static void RemoveFireworkFX(FireworkFXComponent fx)
	{
		if ((bool)fetch)
		{
			fetch.fireworks.Remove(fx);
		}
	}

	public void offsetFireworks(Vector3d offset)
	{
		int count = fireworks.Count;
		while (count-- > 0)
		{
			if (fireworks[count] == null)
			{
				fireworks.RemoveAt(count);
			}
		}
		int count2 = fireworks.Count;
		while (count2-- > 0)
		{
			FireworkFXComponent fireworkFXComponent = fireworks[count2];
			_ = fireworkFXComponent.gameObject;
			int num = fireworkFXComponent.particleSystems.Length;
			while (num-- > 0)
			{
				ParticleSystem particleSystem = fireworkFXComponent.particleSystems[num];
				if (particleSystem == null)
				{
					continue;
				}
				ParticleSystem.MainModule main = particleSystem.main;
				if (main.simulationSpace == ParticleSystemSimulationSpace.World && particleSystem.particleCount > 0)
				{
					int maxParticles = main.maxParticles;
					if (fireworkPSs.Length < maxParticles)
					{
						fireworkPSs = new ParticleSystem.Particle[maxParticles];
					}
					int particles = particleSystem.GetParticles(fireworkPSs);
					int num2 = particles;
					while (num2-- > 0)
					{
						fireworkPSs[num2].position = (Vector3d)fireworkPSs[num2].position + offset;
					}
					particleSystem.SetParticles(fireworkPSs, particles);
				}
			}
		}
	}
}
