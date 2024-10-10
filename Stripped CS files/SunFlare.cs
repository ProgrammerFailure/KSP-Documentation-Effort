using System;
using UnityEngine;

public class SunFlare : MonoBehaviour
{
	public static SunFlare Instance;

	public Transform target;

	public CelestialBody sun;

	public LensFlare sunFlare;

	public double double_0;

	public Vector3d sunDirection;

	public AnimationCurve brightnessCurve;

	public float brightnessMultiplier = 1f;

	public virtual void Awake()
	{
		Instance = this;
	}

	public virtual void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LateUpdate()
	{
		sunDirection = ((Vector3d)target.position - ScaledSpace.LocalToScaledSpace(sun.position)).normalized;
		base.transform.forward = sunDirection;
		if (PlanetariumCamera.fetch.target != null && (HighLogic.LoadedScene == GameScenes.TRACKSTATION || HighLogic.LoadedScene == GameScenes.FLIGHT))
		{
			bool state = true;
			for (int i = 0; i < PlanetariumCamera.fetch.targets.Count; i++)
			{
				MapObject mapObject = PlanetariumCamera.fetch.targets[i];
				if (mapObject.type != MapObject.ObjectType.CelestialBody || !mapObject.GetComponent<SphereCollider>() || !mapObject.GetComponent<MeshRenderer>().enabled || mapObject.transform.localScale.x < 1f || mapObject.transform.localScale.x >= 3f)
				{
					continue;
				}
				Vector3d vector3d = PlanetariumCamera.fetch.transform.position - mapObject.transform.position;
				float radius = mapObject.GetComponent<SphereCollider>().radius;
				double num = 2.0 * Vector3d.Dot(-sunDirection, vector3d);
				double num2 = Vector3d.Dot(vector3d, vector3d) - (double)(radius * radius);
				double num3 = num * num - 4.0 * num2;
				if (num3 >= 0.0)
				{
					double num4 = (0.0 - num + Math.Sqrt(num3)) * 0.5;
					double num5 = (0.0 - num - Math.Sqrt(num3)) * 0.5;
					if (num4 >= 0.0 && num5 >= 0.0)
					{
						state = false;
					}
				}
			}
			SunlightEnabled(state);
		}
		sunFlare.brightness = brightnessMultiplier * brightnessCurve.Evaluate((float)(1.0 / (Vector3d.Distance(target.position, ScaledSpace.LocalToScaledSpace(sun.position)) / (double_0 * (double)ScaledSpace.InverseScaleFactor))));
	}

	public void SunlightEnabled(bool state)
	{
		sunFlare.enabled = state;
	}
}
