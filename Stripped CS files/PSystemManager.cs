using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KSP/SolarSystem/PSystemManager")]
public class PSystemManager : MonoBehaviour
{
	public string localSpaceName;

	public string scaledSpaceName;

	public float scaledSpaceFactor;

	public PlanetariumCamera scaledSpaceCamera;

	public Sun sun;

	public SunFlare sunFlare;

	public PSystem systemPrefab;

	public Transform localParent;

	public Transform scaledParent;

	public ScaledSpace scaledSpace;

	public static Dictionary<CelestialBody, OrbitRendererData> OrbitRendererDataCache;

	public EventVoid onPSystemReady = new EventVoid("OnPSystemReady");

	public double maximumSurfaceGeeASL;

	public static PSystemManager Instance { get; set; }

	public List<CelestialBody> localBodies { get; set; }

	public List<GameObject> scaledBodies { get; set; }

	public EventVoid OnPSystemReady => onPSystemReady;

	public double MaximumSurfaceGeeASL => maximumSurfaceGeeASL;

	public void Reset()
	{
		localSpaceName = "localSpace";
		scaledSpaceName = "scaledSpace";
		scaledSpaceFactor = 6000f;
	}

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		OrbitRendererDataCache = new Dictionary<CelestialBody, OrbitRendererData>();
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public IEnumerator Start()
	{
		SetupPSystem();
		yield return null;
		onPSystemReady.Fire();
	}

	public void SetupPSystem()
	{
		if (systemPrefab == null)
		{
			Debug.LogError("PSystemManager: systemPrefab is null!");
			return;
		}
		if (systemPrefab.rootBody == null)
		{
			Debug.LogError("PSystemManager: systemPrefab root body is null!");
			return;
		}
		GameObject gameObject = new GameObject(localSpaceName);
		localParent = gameObject.transform;
		SetupLocalSpace(gameObject);
		GameObject gameObject2 = new GameObject(scaledSpaceName);
		scaledParent = gameObject2.transform;
		SetupScaledSpace(gameObject2);
		localBodies = new List<CelestialBody>();
		scaledBodies = new List<GameObject>();
		OrbitRendererDataCache.Clear();
		SpawnBody(null, systemPrefab.rootBody);
		PostSetup();
	}

	public void SetupLocalSpace(GameObject localSpace)
	{
		localSpace.AddComponent<LocalSpace>();
		localSpace.AddComponent<FloatingOrigin>();
		localSpace.AddComponent<Krakensbane>();
	}

	public void SetupScaledSpace(GameObject scaledObject)
	{
		scaledSpace = scaledObject.AddComponent<ScaledSpace>();
		scaledSpace.scaleFactor = scaledSpaceFactor;
		scaledSpace.originTarget = scaledSpaceCamera.transform;
		scaledSpaceCamera.transform.parent = scaledSpace.transform;
	}

	public void SpawnBody(CelestialBody parent, PSystemBody body)
	{
		if (body.celestialBody == null)
		{
			Debug.LogError("PSystemBody (" + body.name + "): celestialBody is null!");
			return;
		}
		if (body.scaledVersion == null)
		{
			Debug.LogError("PSystemBody (" + body.name + "): scaledVersion is null!");
			return;
		}
		CelestialBody celestialBody = UnityEngine.Object.Instantiate(body.celestialBody);
		celestialBody.orbitingBodies = new List<CelestialBody>();
		celestialBody.transform.parent = localParent;
		celestialBody.gameObject.name = body.celestialBody.bodyName;
		celestialBody.flightGlobalsIndex = body.flightGlobalsIndex;
		if (parent == null)
		{
			sun.sun = celestialBody;
			sunFlare.sun = celestialBody;
			Planetarium.fetch.Sun = celestialBody;
		}
		else
		{
			parent.orbitingBodies.Add(celestialBody);
		}
		if (celestialBody.isHomeWorld)
		{
			Planetarium.fetch.Home = celestialBody;
		}
		OrbitDriver component = celestialBody.gameObject.GetComponent<OrbitDriver>();
		if (component != null)
		{
			component.orbit.referenceBody = parent;
			OrbitRenderer component2 = celestialBody.gameObject.GetComponent<OrbitRenderer>();
			OrbitRendererData orbitRendererData = new OrbitRendererData(celestialBody);
			orbitRendererData.orbitColor = component2.orbitColor;
			orbitRendererData.nodeColor = component2.nodeColor;
			orbitRendererData.lowerCamVsSmaRatio = component2.lowerCamVsSmaRatio;
			orbitRendererData.upperCamVsSmaRatio = component2.upperCamVsSmaRatio;
			OrbitRendererDataCache.Add(celestialBody, orbitRendererData);
			UnityEngine.Object.DestroyImmediate(component2);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(body.scaledVersion);
		gameObject.transform.parent = scaledParent;
		gameObject.name = body.celestialBody.bodyName;
		celestialBody.MapObject = ScaledMovement.Create(celestialBody.name, celestialBody, gameObject);
		PlanetariumCamera.fetch.targets.Add(celestialBody.MapObject);
		if (body.planetariumCameraInitial)
		{
			PlanetariumCamera.fetch.target = celestialBody.MapObject;
			PlanetariumCamera.fetch.initialTarget = celestialBody.MapObject;
			Planetarium.fetch.CurrentMainBody = celestialBody;
		}
		if (body.pqsVersion != null)
		{
			GClass4 gClass = UnityEngine.Object.Instantiate(body.pqsVersion);
			gClass.name = celestialBody.bodyName;
			gClass.transform.parent = celestialBody.transform;
			gClass.transform.localPosition = Vector3.zero;
			gClass.transform.localRotation = Quaternion.identity;
			gClass.isAlive = false;
			gClass.SetTarget(FlightCamera.fetch.transform);
			gClass.SetSecondaryTarget(FlightCamera.fetch.transform);
			celestialBody.pqsController = gClass;
		}
		SetupLocal(celestialBody, celestialBody.pqsController);
		SetupScaled(celestialBody, gameObject);
		localBodies.Add(celestialBody);
		scaledBodies.Add(gameObject);
		celestialBody.scaledBody = gameObject;
		foreach (PSystemBody child in body.children)
		{
			SpawnBody(celestialBody, child);
		}
	}

	public void SetBodyReferences(CelestialBody cb, GameObject scaled)
	{
		if (PlanetariumCamera.fetch != null)
		{
			PlanetariumCamera.fetch.targets.Add(scaled.GetComponent<ScaledMovement>());
		}
	}

	public void SetupLocal(CelestialBody cb, GClass4 pqs)
	{
		_ = pqs == null;
	}

	public void SetupScaled(CelestialBody cb, GameObject scaled)
	{
		ScaledSpaceFader componentInChildren = scaled.GetComponentInChildren<ScaledSpaceFader>();
		if (componentInChildren != null)
		{
			componentInChildren.celestialBody = cb;
		}
		AtmosphereFromGround componentInChildren2 = scaled.GetComponentInChildren<AtmosphereFromGround>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.planet = cb;
			componentInChildren2.sunLight = sun.gameObject;
			componentInChildren2.mainCamera = scaledSpaceCamera.transform;
		}
		SunCoronas[] componentsInChildren = scaled.GetComponentsInChildren<SunCoronas>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].m_Camera = scaledSpaceCamera.GetComponent<Camera>();
		}
	}

	public void PostSetup()
	{
		localBodies.Sort((CelestialBody a, CelestialBody b) => (a.flightGlobalsIndex == -1) ? a.bodyName.CompareTo(b.bodyName) : a.flightGlobalsIndex.CompareTo(b.flightGlobalsIndex));
		maximumSurfaceGeeASL = double.MinValue;
		foreach (CelestialBody localBody in localBodies)
		{
			FlightGlobals.fetch.bodies.Add(localBody);
			if (localBody.hasSolidSurface)
			{
				maximumSurfaceGeeASL = Math.Max(maximumSurfaceGeeASL, localBody.GeeASL);
			}
		}
	}
}
