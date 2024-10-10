using System;
using System.Collections.Generic;
using UnityEngine;

public class Planetarium : MonoBehaviour
{
	public struct CelestialFrame
	{
		public Vector3d vector3d_0;

		public Vector3d vector3d_1;

		public Vector3d vector3d_2;

		public QuaternionD Rotation => new QuaternionD(vector3d_0, vector3d_1, vector3d_2);

		public Vector3d WorldToLocal(Vector3d r)
		{
			double x = Vector3d.Dot(r, vector3d_0);
			double y = Vector3d.Dot(r, vector3d_1);
			double z = Vector3d.Dot(r, vector3d_2);
			return new Vector3d(x, y, z);
		}

		public Vector3d LocalToWorld(Vector3d r)
		{
			return r.x * vector3d_0 + r.y * vector3d_1 + r.z * vector3d_2;
		}

		public static void SetFrame(double double_0, double double_1, double double_2, ref CelestialFrame cf)
		{
			double num = Math.Cos(double_0);
			double num2 = Math.Sin(double_0);
			double num3 = Math.Cos(double_1);
			double num4 = Math.Sin(double_1);
			double num5 = Math.Cos(double_2);
			double num6 = Math.Sin(double_2);
			cf.vector3d_0 = new Vector3d(num * num5 - num2 * num3 * num6, num2 * num5 + num * num3 * num6, num4 * num6);
			cf.vector3d_1 = new Vector3d((0.0 - num) * num6 - num2 * num3 * num5, (0.0 - num2) * num6 + num * num3 * num5, num4 * num5);
			cf.vector3d_2 = new Vector3d(num2 * num4, (0.0 - num) * num4, num3);
		}

		public static void OrbitalFrame(double double_0, double Inc, double ArgPe, ref CelestialFrame cf)
		{
			double_0 *= Math.PI / 180.0;
			Inc *= Math.PI / 180.0;
			ArgPe *= Math.PI / 180.0;
			SetFrame(double_0, Inc, ArgPe, ref cf);
		}

		public static void PlanetaryFrame(double ra, double dec, double rot, ref CelestialFrame cf)
		{
			ra = (ra - 90.0) * Math.PI / 180.0;
			dec = (dec - 90.0) * Math.PI / 180.0;
			rot = (rot + 90.0) * Math.PI / 180.0;
			SetFrame(ra, dec, rot, ref cf);
		}
	}

	public List<OrbitDriver> orbits;

	public double time;

	public double timeScale = 1.0;

	public bool pause;

	public static Planetarium fetch;

	public QuaternionD rotation;

	public double inverseRotAngle;

	public static CelestialFrame Zup;

	public CelestialBody Sun;

	public CelestialBody Home;

	public CelestialBody CurrentMainBody;

	public double fixedDeltaTime = 0.02;

	public static List<OrbitDriver> Orbits
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.orbits;
		}
	}

	public static double TimeScale
	{
		get
		{
			if (!fetch)
			{
				return 1.0;
			}
			return fetch.timeScale;
		}
		set
		{
			if ((bool)fetch)
			{
				fetch.timeScale = value;
			}
		}
	}

	public static bool Pause
	{
		get
		{
			if (!fetch)
			{
				return false;
			}
			return fetch.pause;
		}
	}

	public static QuaternionD Rotation
	{
		get
		{
			return fetch.rotation;
		}
		set
		{
			fetch.rotation = value;
		}
	}

	public static double InverseRotAngle
	{
		get
		{
			return fetch.inverseRotAngle;
		}
		set
		{
			fetch.inverseRotAngle = value;
		}
	}

	public static Vector3d up => Rotation * new Vector3d(0.0, 1.0, 0.0);

	public static Vector3d forward => Rotation * new Vector3d(0.0, 0.0, 1.0);

	public static Vector3d right => Rotation * new Vector3d(1.0, 0.0, 0.0);

	public static void ZupAtT(double double_0, CelestialBody body, ref CelestialFrame tempZup)
	{
		double num = (body.initialRotation + 360.0 * body.rotPeriodRecip * double_0) % 360.0;
		if (body.inverseRotation)
		{
			double rot = (num - body.directRotAngle) % 360.0;
			CelestialFrame.PlanetaryFrame(0.0, 90.0, rot, ref tempZup);
		}
		else
		{
			tempZup.vector3d_0 = Zup.vector3d_0;
			tempZup.vector3d_1 = Zup.vector3d_1;
			tempZup.vector3d_2 = Zup.vector3d_2;
		}
	}

	public static Vector3d SphericalVector(double lat, double lon)
	{
		double num = Math.Cos(lat);
		double z = Math.Sin(lat);
		double num2 = Math.Cos(lon);
		double num3 = Math.Sin(lon);
		return new Vector3d(num * num2, num * num3, z);
	}

	public void Awake()
	{
		if (fetch != null)
		{
			throw new Exception("Don't try to instantiate the singleton Planetarium class more than once!");
		}
		fetch = this;
		CelestialFrame.PlanetaryFrame(0.0, 90.0, inverseRotAngle, ref Zup);
		rotation = QuaternionD.Inverse(Zup.Rotation).swizzle;
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void FixedUpdate()
	{
		if (!pause)
		{
			time += fixedDeltaTime * timeScale;
			if ((bool)Sun)
			{
				CurrentMainBody = FindRootBody(Sun);
				UpdateCBsRecursive(CurrentMainBody);
			}
		}
	}

	public void Update()
	{
		if (HighLogic.CurrentGame != null && !pause)
		{
			HighLogic.CurrentGame.CrewRoster.PCMUpdate(time);
		}
	}

	public void UpdateCBs()
	{
		if ((bool)Sun)
		{
			CurrentMainBody = FindRootBody(Sun);
			UpdateCBsRecursive(CurrentMainBody);
		}
	}

	public CelestialBody FindRootBody(CelestialBody cb)
	{
		int num = 0;
		CelestialBody celestialBody;
		while (true)
		{
			if (num < cb.orbitingBodies.Count)
			{
				celestialBody = cb.orbitingBodies[num];
				if ((bool)celestialBody.orbitDriver && celestialBody.orbitDriver.reverse)
				{
					break;
				}
				num++;
				continue;
			}
			return cb;
		}
		return FindRootBody(celestialBody);
	}

	public void UpdateCBsRecursive(CelestialBody cb)
	{
		cb.CBUpdate();
		int count = cb.orbitingBodies.Count;
		for (int i = 0; i < count; i++)
		{
			CelestialBody celestialBody = cb.orbitingBodies[i];
			if (!celestialBody.orbitDriver || !celestialBody.orbitDriver.reverse)
			{
				UpdateCBsRecursive(celestialBody);
			}
		}
		if ((bool)cb.orbitDriver && cb.orbitDriver.reverse)
		{
			UpdateCBsRecursive(cb.referenceBody);
		}
	}

	public static double GetUniversalTime()
	{
		if (!(fetch != null))
		{
			return HighLogic.CurrentGame.UniversalTime;
		}
		return fetch.time;
	}

	public static void SetUniversalTime(double t)
	{
		if (fetch != null)
		{
			fetch.time = t;
		}
	}

	public static bool FrameIsRotating()
	{
		return findRotatingBodiesRecursive(fetch.Sun);
	}

	public static bool findRotatingBodiesRecursive(CelestialBody cb)
	{
		if (cb.rotates && cb.inverseRotation)
		{
			return true;
		}
		int num = 0;
		while (true)
		{
			if (num < cb.orbitingBodies.Count)
			{
				if (findRotatingBodiesRecursive(cb.orbitingBodies[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
