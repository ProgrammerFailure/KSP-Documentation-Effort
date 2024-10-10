using System;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceFX : MonoBehaviour
{
	[SerializeField]
	public ParticleSystem srfCloud;

	public ParticleSystem.MainModule srfCloudMain;

	public ParticleSystem.Particle[] cloudParticles;

	public int cloudCount;

	[SerializeField]
	public ParticleSystem srfDust;

	public ParticleSystem.MainModule srfDustMain;

	public ParticleSystem.Particle[] dustParticles;

	public int dustCount;

	[SerializeField]
	public ParticleSystem srfWake;

	public ParticleSystem.MainModule srfWakeMain;

	public int wakeCount;

	public float fxScale = 0.5f;

	[SerializeField]
	public float pushThreshold = 0.5f;

	[SerializeField]
	public float pushScale = 5f;

	[SerializeField]
	public float linger = 10f;

	public List<ModuleSurfaceFX> sources = new List<ModuleSurfaceFX>();

	public GameObject prefab;

	public bool atmosphere;

	public static float mergeThreshold = 5f;

	public static List<SurfaceFX> fxs;

	public Transform trf;

	public Vector3 Vsrf;

	public Vector3 upAxis;

	public Vector3 barycenter;

	public float lastUpdate;

	public ModuleSurfaceFX leadSource { get; set; }

	public float ScaledFX => fxScale;

	public static SurfaceFX FindNearestFX(ModuleSurfaceFX src, Vector3 wPos)
	{
		if (fxs == null)
		{
			fxs = new List<SurfaceFX>();
		}
		SurfaceFX result = null;
		float num = mergeThreshold;
		int count = fxs.Count;
		while (count-- > 0)
		{
			if (!(fxs[count].leadSource == src) && (fxs[count].trf.position - wPos).sqrMagnitude < num)
			{
				result = fxs[count];
			}
		}
		return result;
	}

	public void Terminate()
	{
		fxs.Remove(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Awake()
	{
		trf = base.transform;
		fxs.Add(this);
	}

	public void OnDestroy()
	{
		fxs.Remove(this);
		FloatingOrigin.UnregisterParticleSystem(srfCloud);
		FloatingOrigin.UnregisterParticleSystem(srfDust);
		FloatingOrigin.UnregisterParticleSystem(srfWake);
	}

	public void AddSource(ModuleSurfaceFX src)
	{
		if (sources.Count == 0)
		{
			leadSource = src;
		}
		sources.AddUnique(src);
	}

	public void RemoveSource(ModuleSurfaceFX src)
	{
		sources.Remove(src);
		if (src == leadSource)
		{
			if (sources.Count != 0)
			{
				leadSource = sources[0];
			}
			else
			{
				leadSource = null;
			}
		}
	}

	public void Start()
	{
		cloudParticles = new ParticleSystem.Particle[srfCloud.main.maxParticles * 2];
		dustParticles = new ParticleSystem.Particle[srfDust.main.maxParticles * 2];
		upAxis = FlightGlobals.ActiveVessel.upAxis;
		barycenter = Vector3.zero;
		Vsrf = Vector3.zero;
		srfCloudMain = srfCloud.main;
		srfWakeMain = srfWake.main;
		srfDustMain = srfDust.main;
		FloatingOrigin.RegisterParticleSystem(srfCloud);
		FloatingOrigin.RegisterParticleSystem(srfDust);
		FloatingOrigin.RegisterParticleSystem(srfWake);
	}

	public void LateUpdate()
	{
		fxScale = 0f;
		int count = sources.Count;
		while (count-- > 0)
		{
			lastUpdate = Time.realtimeSinceStartup;
			fxScale = Mathf.Clamp01(fxScale + sources[count].ScaledFX);
		}
		if (sources.Count > 0)
		{
			atmosphere = leadSource.part.vessel.mainBody.atmosphere;
			if (sources.Count > 1)
			{
				barycenter = GetWeightedAvgVector((int i) => sources[i].point, (int i) => sources[i].ScaledFX);
				upAxis = GetWeightedAvgVector((int i) => sources[i].normal, (int i) => sources[i].ScaledFX);
				Vsrf = GetWeightedAvgVector((int i) => sources[i].Vsrf, (int i) => sources[i].ScaledFX) * pushScale;
			}
			else
			{
				barycenter = leadSource.point;
				upAxis = leadSource.normal;
				Vsrf = leadSource.Vsrf;
			}
			trf.position = barycenter;
			trf.rotation = Quaternion.LookRotation(Vsrf, upAxis);
		}
		if (Time.realtimeSinceStartup > lastUpdate + linger)
		{
			Terminate();
		}
		else if (fxScale > 0f)
		{
			if (!srfCloud.isPlaying && atmosphere)
			{
				srfCloud.Play();
			}
			if (!srfDust.isPlaying)
			{
				srfDust.Play();
			}
			if (!srfWake.isPlaying)
			{
				srfWake.Play();
			}
			if (!srfCloud.emission.enabled && atmosphere)
			{
				ParticleSystem.EmissionModule emission = srfCloud.emission;
				emission.enabled = true;
			}
			if (!srfDust.emission.enabled)
			{
				ParticleSystem.EmissionModule emission2 = srfDust.emission;
				emission2.enabled = true;
			}
			if (!srfWake.emission.enabled)
			{
				ParticleSystem.EmissionModule emission3 = srfWake.emission;
				emission3.enabled = true;
			}
			if (atmosphere)
			{
				srfCloudMain.startColor = srfCloudMain.startColor.color.smethod_0(Mathf.Lerp(0f, 0.5f, fxScale));
				cloudCount = srfCloud.GetParticles(cloudParticles);
				int num = cloudCount;
				while (num-- > 0)
				{
					UpdateParticle(ref cloudParticles[num], 1f / srfCloudMain.startLifetime.constant);
				}
				srfCloud.SetParticles(cloudParticles, cloudCount);
			}
			srfDustMain.startColor = srfDustMain.startColor.color.smethod_0(Mathf.Lerp(0f, 0.5f, fxScale));
			dustCount = srfDust.GetParticles(dustParticles);
			int num2 = dustCount;
			while (num2-- > 0)
			{
				UpdateParticle(ref dustParticles[num2], 1f / srfDustMain.startLifetime.constant);
			}
			srfDust.SetParticles(dustParticles, dustCount);
			srfWakeMain.startColor = srfWakeMain.startColor.color.smethod_0(Mathf.Lerp(0f, 0.5f, fxScale));
		}
		else
		{
			if (srfCloud.emission.enabled)
			{
				ParticleSystem.EmissionModule emission4 = srfCloud.emission;
				emission4.enabled = false;
			}
			if (srfDust.emission.enabled)
			{
				ParticleSystem.EmissionModule emission5 = srfDust.emission;
				emission5.enabled = false;
			}
			if (srfWake.emission.enabled)
			{
				ParticleSystem.EmissionModule emission6 = srfWake.emission;
				emission6.enabled = false;
			}
		}
	}

	public void UpdateParticle(ref ParticleSystem.Particle p, float lifeTimeThreshold)
	{
		p.velocity += Vsrf * Mathf.Pow(p.remainingLifetime * lifeTimeThreshold, pushThreshold);
	}

	public Vector3 GetWeightedAvgVector(Func<int, Vector3> getVector, Func<int, float> getWeight)
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		int count = sources.Count;
		while (count-- > 0)
		{
			if (sources[count] == null)
			{
				sources.RemoveAt(count);
				continue;
			}
			float num2 = getWeight(count);
			zero += getVector(count) * num2;
			num += num2;
		}
		if (num != 0f)
		{
			return zero / num;
		}
		return zero;
	}
}
