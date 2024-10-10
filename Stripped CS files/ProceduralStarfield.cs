using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ProceduralStarfield : MonoBehaviour
{
	[Serializable]
	public class StarClass
	{
		public float minSize = 0.5f;

		public float maxSize = 1f;

		public float minMagnitude = 0.5f;

		public float maxMagnitude = 1f;

		public Color color = Color.white;

		public StarClass()
		{
		}

		public StarClass(float minSize, float maxSize, float minMag, float maxMag, Color color)
		{
			this.minSize = minSize;
			this.maxSize = maxSize;
			minMagnitude = minMag;
			maxMagnitude = maxMag;
			this.color = color;
		}
	}

	public int seed;

	public Vector3 fieldSize;

	public int numStars;

	public bool useStaticStarSize;

	public float staticStarSize;

	public bool useExclusionZone;

	public Vector3 exclusionZone;

	public Vector3 exclusionZoneOffset;

	public StarClass[] starClasses;

	public Material starMaterial;

	public Vector3 exclusionZoneMin;

	public Vector3 exclusionZoneMax;

	public void Reset()
	{
		seed = 123456;
		fieldSize = new Vector3(1000f, 1000f, 1000f);
		numStars = 10000;
		useStaticStarSize = false;
		staticStarSize = 1f;
		useExclusionZone = true;
		exclusionZone = new Vector3(250f, 250f, 250f);
		exclusionZoneOffset = Vector3.zero;
		starClasses = new StarClass[3];
		starClasses[0] = new StarClass(0.5f, 1f, 0.5f, 0.8f, new Color(1f, 1f, 0.8f));
		starClasses[1] = new StarClass(1f, 2f, 0.3f, 0.6f, new Color(1f, 0.4f, 0.1f));
		starClasses[2] = new StarClass(0.5f, 2f, 0.8f, 1f, new Color(0.4f, 0.4f, 1f));
	}

	public void Start()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		CreateStarFields();
		Debug.Log("Starfield created: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F2") + "s");
	}

	public void CreateStarFields()
	{
		int num = numStars;
		int num2 = 0;
		int num3 = 0;
		UnityEngine.Random.InitState(seed);
		exclusionZoneMin = new Vector3(0f - exclusionZone.x + exclusionZoneOffset.x, 0f - exclusionZone.y + exclusionZoneOffset.y, 0f - exclusionZone.z + exclusionZoneOffset.z);
		exclusionZoneMax = new Vector3(exclusionZone.x + exclusionZoneOffset.x, exclusionZone.y + exclusionZoneOffset.y, exclusionZone.z + exclusionZoneOffset.z);
		while (num > 0)
		{
			if (num >= 16000)
			{
				num2 = 16000;
				num -= 16000;
			}
			else
			{
				num2 = num;
				num = 0;
			}
			GameObject obj = new GameObject();
			obj.name = "Emitter" + num3;
			obj.transform.parent = base.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.rotation = Quaternion.identity;
			obj.AddComponent<ParticleSystem>().SetParticles(CreateStarsWithNewSystem(num2), num2);
			ParticleSystemRenderer component = obj.GetComponent<ParticleSystemRenderer>();
			component.material = starMaterial;
			component.receiveShadows = false;
			component.shadowCastingMode = ShadowCastingMode.Off;
			num3++;
		}
	}

	public ParticleSystem.Particle[] CreateStarsWithNewSystem(int numStars)
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[numStars];
		int max = starClasses.Length;
		StarClass starClass = null;
		float num = 0f;
		Vector3 vector = Vector3.zero;
		bool flag = true;
		for (int i = 0; i < numStars; i++)
		{
			starClass = starClasses[UnityEngine.Random.Range(0, max)];
			num = UnityEngine.Random.Range(starClass.minMagnitude, starClass.maxMagnitude);
			array[i] = default(ParticleSystem.Particle);
			array[i].startColor = starClass.color * num;
			array[i].rotation = UnityEngine.Random.value * 360f;
			if (useStaticStarSize)
			{
				array[i].startSize = staticStarSize;
			}
			else
			{
				array[i].startSize = UnityEngine.Random.Range(starClass.minSize, starClass.maxSize);
			}
			if (useExclusionZone)
			{
				flag = true;
				while (flag)
				{
					vector = UnityEngine.Random.insideUnitSphere;
					vector.Scale(fieldSize);
					flag = ExclusionZoneTest(vector);
				}
			}
			else
			{
				vector = UnityEngine.Random.insideUnitSphere;
				vector.Scale(fieldSize);
			}
			array[i].position = base.transform.position + vector;
		}
		return array;
	}

	public bool ExclusionZoneTest(Vector3 position)
	{
		if (!(position.x < exclusionZoneMin.x) && !(position.x > exclusionZoneMax.x) && !(position.y < exclusionZoneMin.y) && !(position.y > exclusionZoneMax.y) && !(position.z < exclusionZoneMin.z))
		{
			return !(position.z > exclusionZoneMax.z);
		}
		return false;
	}
}
