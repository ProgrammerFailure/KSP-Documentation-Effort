using System;
using System.Collections;
using System.Collections.Generic;
using Expansions;
using ns9;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgradeables;

public class PSystemSetup : MonoBehaviour
{
	[Serializable]
	public class SpaceCenterFacility
	{
		[Serializable]
		public class SpawnPoint
		{
			public string name;

			public string spawnTransformURL;

			public double latitude;

			public double longitude;

			public double altitude;

			public bool latlonaltSet;

			[NonSerialized]
			public SpaceCenterFacility host;

			public Transform spawnPointTransform;

			public void Setup(SpaceCenterFacility host)
			{
				this.host = host;
			}

			public Transform GetSpawnPointTransform()
			{
				if (spawnPointTransform == null)
				{
					spawnPointTransform = host.facilityTransform.Find(spawnTransformURL);
				}
				if (spawnPointTransform == null)
				{
					Debug.Log("[PSystemSetup::SpaceCenterFacility::SpawnPoint]: Cannot find a transform named '" + spawnTransformURL + "' on '" + name + "'");
					return null;
				}
				return spawnPointTransform;
			}

			public void GetSpawnPointLatLonAlt(out double latitude, out double longitude, out double altitude)
			{
				latitude = this.latitude;
				longitude = this.longitude;
				altitude = this.altitude;
			}

			public void SetSpawnPointLatLonAlt()
			{
				if (spawnPointTransform == null)
				{
					spawnPointTransform = host.facilityTransform.Find(spawnTransformURL);
				}
				if (spawnPointTransform == null)
				{
					Debug.Log("[PSystemSetup::SpaceCenterFacility::SpawnPoint]: Cannot find a transform named '" + spawnTransformURL + "' on '" + name + "'");
					latlonaltSet = false;
				}
				else if (host.hostBody != null)
				{
					host.hostBody.GetLatLonAlt(spawnPointTransform.position, out latitude, out longitude, out altitude);
					latlonaltSet = true;
				}
				else
				{
					Debug.Log("[PSystemSetup::SpaceCenterFacility::SpawnPoint]: (" + name + ") Unable to set Latitude, Longitude, Altitude");
					latlonaltSet = false;
				}
			}
		}

		public string name;

		public string pqsName;

		public string facilityTransformName;

		public string facilityName;

		public string facilityDisplayName;

		public GClass4 facilityPQS;

		public CelestialBody hostBody;

		public Transform facilityTransform;

		public EditorFacility editorFacility;

		public SpawnPoint[] spawnPoints;

		public UpgradeableObject upgradeableObject;

		public void Setup(GClass4[] PQSs)
		{
			foreach (GClass4 gClass in PQSs)
			{
				if (gClass.gameObject.name == pqsName)
				{
					facilityPQS = gClass;
					hostBody = gClass.GetComponentInParent<CelestialBody>();
					facilityTransform = gClass.transform.Find(facilityTransformName);
					string[] array = facilityTransformName.Split('/');
					facilityName = array[array.Length - 1];
					if (facilityTransform != null)
					{
						break;
					}
				}
			}
			if (facilityTransform == null)
			{
				Debug.Log("[PSystemSetup::SpaceCenterFacility]: Cannot find facility named '" + facilityTransformName + "' on pqs '" + pqsName + "'");
			}
			else
			{
				SpawnPoint[] array2 = spawnPoints;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].Setup(this);
				}
			}
		}

		public void SetSpawnPointsLatLonAlt()
		{
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				spawnPoints[i].SetSpawnPointLatLonAlt();
			}
		}

		public SpawnPoint GetSpawnPoint(string spawnPointName)
		{
			SpawnPoint[] array = spawnPoints;
			int num = 0;
			SpawnPoint spawnPoint;
			while (true)
			{
				if (num < array.Length)
				{
					spawnPoint = array[num];
					if (spawnPoint.name == spawnPointName)
					{
						break;
					}
					num++;
					continue;
				}
				Debug.Log("[PSystemSetup::SpaceCenterFacility]: Cannot find spawnpoint named '" + spawnPointName + "'");
				return null;
			}
			return spawnPoint;
		}

		public float GetFacilityDamage()
		{
			return ScenarioDestructibles.GetFacilityDamage(facilityName);
		}

		public float GetFacilityLevel()
		{
			return ScenarioUpgradeableFacilities.GetFacilityLevel(facilityName);
		}

		public bool IsLaunchFacility()
		{
			if (spawnPoints != null)
			{
				return spawnPoints.Length != 0;
			}
			return false;
		}
	}

	public GameScenes nextScene = GameScenes.MAINMENU;

	public bool loadTestScene;

	public string pqsToActivate = "Kerbin";

	public string pqsTransformToCache = "KSC";

	[SerializeField]
	public SpaceCenterFacility[] facilities;

	[SerializeField]
	public List<SpaceCenterFacility> spaceCenterFacilityLaunchSites;

	public GClass4[] pqsArray;

	[SerializeField]
	public LaunchSite[] stocklaunchsites;

	[SerializeField]
	public List<LaunchSite> launchsites;

	[SerializeField]
	public GameObject mobileLaunchSitePrefab;

	public GClass4 pqs;

	public PQSMod_CelestialBodyTransform cb;

	public Transform scTransform;

	public static PSystemSetup Instance { get; set; }

	public List<SpaceCenterFacility> SpaceCenterFacilityLaunchSites => spaceCenterFacilityLaunchSites;

	public SpaceCenterFacility[] SpaceCenterFacilities
	{
		get
		{
			return facilities;
		}
		set
		{
			facilities = value;
		}
	}

	public LaunchSite[] StockLaunchSites => stocklaunchsites;

	public LaunchSite[] NonStockLaunchSites
	{
		get
		{
			List<LaunchSite> list = new List<LaunchSite>();
			for (int i = 0; i < launchsites.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < stocklaunchsites.Length; j++)
				{
					if (stocklaunchsites[j].name == launchsites[i].name)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(launchsites[i]);
				}
			}
			return list.ToArray();
		}
	}

	public List<LaunchSite> LaunchSites => launchsites;

	public Transform SCTransform => scTransform;

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
		GameEvents.OnUpgradeableObjLevelChange.Add(onUpgradeableObjLevelChange);
		PSystemManager.Instance.OnPSystemReady.Add(OnPSystemReady);
		spaceCenterFacilityLaunchSites = new List<SpaceCenterFacility>();
		launchsites = new List<LaunchSite>();
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			mobileLaunchSitePrefab = BundleLoader.LoadAsset<PQSCity2>("makinghistory_assets", "Assets/Expansions/Missions/Scenery/MobileLaunchPad.prefab") as GameObject;
		}
		else
		{
			mobileLaunchSitePrefab = null;
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		GameEvents.OnUpgradeableObjLevelChange.Remove(onUpgradeableObjLevelChange);
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void onUpgradeableObjLevelChange(UpgradeableObject upgradeObject, int level)
	{
		for (int i = 0; i < spaceCenterFacilityLaunchSites.Count; i++)
		{
			if (spaceCenterFacilityLaunchSites[i].facilityTransform != null)
			{
				if (spaceCenterFacilityLaunchSites[i].upgradeableObject == null)
				{
					spaceCenterFacilityLaunchSites[i].upgradeableObject = spaceCenterFacilityLaunchSites[i].facilityTransform.GetComponent<UpgradeableObject>();
				}
				if (spaceCenterFacilityLaunchSites[i].upgradeableObject != null && upgradeObject == spaceCenterFacilityLaunchSites[i].upgradeableObject)
				{
					spaceCenterFacilityLaunchSites[i].SetSpawnPointsLatLonAlt();
				}
			}
		}
	}

	public IEnumerator SetupSystem()
	{
		yield return null;
		PSystemManager.Instance.OnPSystemReady.Remove(OnPSystemReady);
		pqs = null;
		cb = null;
		scTransform = null;
		pqsArray = (GClass4[])UnityEngine.Object.FindObjectsOfType(typeof(GClass4));
		GClass4[] array = pqsArray;
		foreach (GClass4 gClass in array)
		{
			if (gClass.gameObject.name == pqsToActivate)
			{
				pqs = gClass;
				break;
			}
		}
		if (pqs == null)
		{
			Debug.LogError("PSystemSetup: Cannot find PQS of name '" + pqsToActivate + "'!");
			yield break;
		}
		cb = pqs.GetComponentInChildren<PQSMod_CelestialBodyTransform>();
		if (cb == null)
		{
			Debug.LogError("PSystemSetup: PQS '" + pqsToActivate + "' has no celestial body mod!!");
			yield break;
		}
		scTransform = pqs.transform.Find(pqsTransformToCache);
		if (scTransform == null)
		{
			Debug.LogError(GetType().Name + ": Cannot find transform of name '" + pqsTransformToCache + "'!");
			yield break;
		}
		FloatingOrigin.SetOffset(scTransform.position);
		yield return null;
		HighLogic.LoadedSceneIsGame = true;
		Transform sphereTarget = pqs.target;
		pqs.SetTarget(null);
		yield return null;
		if ((bool)Planetarium.fetch)
		{
			Planetarium.SetUniversalTime(0.0);
			Planetarium.fetch.UpdateCBs();
			Planetarium.fetch.pause = true;
			yield return null;
		}
		SetupFacilities();
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = "";
		}
		yield return StartCoroutine(SetupLaunchSites());
		HighLogic.LoadedSceneIsGame = false;
		yield return null;
		pqs.SetTarget(sphereTarget);
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.textInfo.text = "";
		}
		if (!loadTestScene)
		{
			HighLogic.LoadScene(nextScene);
		}
		else
		{
			OnLevelLoaded(nextScene);
		}
	}

	public void OnPSystemReady()
	{
		StartCoroutine(SetupSystem());
	}

	public void SetupFacilities()
	{
		SpaceCenterFacility[] array = facilities;
		foreach (SpaceCenterFacility spaceCenterFacility in array)
		{
			spaceCenterFacility.Setup(pqsArray);
			if (spaceCenterFacility.IsLaunchFacility())
			{
				spaceCenterFacilityLaunchSites.Add(spaceCenterFacility);
			}
		}
	}

	public IEnumerator SetupLaunchSites()
	{
		List<GClass4> spherestoSetup = new List<GClass4>();
		for (int lsI = 0; lsI < stocklaunchsites.Length; lsI++)
		{
			LaunchSite launchSite = stocklaunchsites[lsI];
			if (string.Equals(launchSite.BundleName, "makinghistory_assets") && !ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
			{
				Debug.Log("PSystemSetup: Cannot check launchsite named " + launchSite.name + "- Making History expansion is not installed.");
				continue;
			}
			if ((bool)LoadingBufferMask.Instance)
			{
				LoadingBufferMask.Instance.textInfo.text = "";
			}
			GameObject gameObject = FindPQS(launchSite.pqsName);
			if (gameObject != null)
			{
				Transform transform = gameObject.transform.Find(launchSite.launchSiteTransformURL);
				if (transform != null)
				{
					PQSCity component = transform.GetComponent<PQSCity>();
					if (component != null)
					{
						launchSite.pqsCity = component;
						component.launchSite = launchSite;
					}
					PQSCity2 component2 = transform.GetComponent<PQSCity2>();
					if (component2 != null)
					{
						launchSite.pqsCity2 = component2;
						component2.launchSite = launchSite;
					}
				}
			}
			bool flag = false;
			bool forcePQSPosition = false;
			if (launchSite.pqsCity != null)
			{
				if (launchSite.Setup(launchSite.pqsCity, pqsArray))
				{
					launchsites.Add(launchSite);
					launchSite.pqsCity.launchSite = launchSite;
					flag = true;
				}
			}
			else if (launchSite.pqsCity2 != null)
			{
				if (launchSite.Setup(launchSite.pqsCity2, pqsArray))
				{
					launchsites.Add(launchSite);
					launchSite.pqsCity2.launchSite = launchSite;
					flag = true;
				}
				SpawnAdditionalPrefabs(flag, launchSite, gameObject);
			}
			else
			{
				if (!string.IsNullOrEmpty(launchSite.prefabPath) && !string.IsNullOrEmpty(launchSite.BundleName))
				{
					GameObject gameObject2 = BundleLoader.LoadAsset<PQSCity2>(launchSite.BundleName, launchSite.prefabPath) as GameObject;
					if (gameObject2 != null && gameObject != null)
					{
						GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, gameObject.transform);
						if (gameObject3 != null)
						{
							int num = gameObject3.name.IndexOf("(Clone)", StringComparison.InvariantCultureIgnoreCase);
							if (num != -1)
							{
								gameObject3.name = gameObject3.name.Remove(num);
							}
							launchSite.pqsCity = gameObject3.GetComponent<PQSCity>();
							launchSite.pqsCity2 = gameObject3.GetComponent<PQSCity2>();
							if (launchSite.pqsCity != null)
							{
								launchSite.pqsCity.sphere = gameObject.GetComponent<GClass4>();
								if (launchSite.Setup(launchSite.pqsCity, pqsArray))
								{
									launchsites.Add(launchSite);
									launchSite.pqsCity.launchSite = launchSite;
									flag = true;
								}
							}
							else if (launchSite.pqsCity2 != null)
							{
								launchSite.pqsCity2.sphere = gameObject.GetComponent<GClass4>();
								if (launchSite.Setup(launchSite.pqsCity2, pqsArray))
								{
									launchsites.Add(launchSite);
									launchSite.pqsCity2.launchSite = launchSite;
									flag = true;
								}
							}
							if (flag)
							{
								PQSMod_FlattenArea component3 = gameObject3.GetComponent<PQSMod_FlattenArea>();
								if (component3 != null)
								{
									component3.sphere = gameObject.GetComponent<GClass4>();
									component3.sphere.ResetModList();
									forcePQSPosition = true;
								}
							}
						}
					}
				}
				SpawnAdditionalPrefabs(flag, launchSite, gameObject);
			}
			if (flag)
			{
				spherestoSetup.AddUnique(launchSite.launchsitePQS);
			}
			if (flag)
			{
				if (launchSite.isPQSCity)
				{
					GClass4 sphere2 = launchSite.pqsCity.sphere;
					Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
					Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
					Vector3d vector3d = LatLon.GetSurfaceNVector(cf, launchSite.pqsCity.lat, launchSite.pqsCity.lon) * (sphere2.radius + launchSite.pqsCity.alt);
					GameObject tempObject2 = new GameObject("TempLocation_" + launchSite.pqsCity.name);
					tempObject2.transform.SetParent(sphere2.transform);
					tempObject2.transform.localPosition = vector3d;
					FloatingOrigin.SetOffset(tempObject2.transform.position);
					yield return new WaitForFixedUpdate();
					if (forcePQSPosition)
					{
						ForceInitPQS(sphere2, tempObject2.transform);
						yield return new WaitForSeconds(2f);
					}
					launchSite.pqsCity.Orientate();
					if (forcePQSPosition)
					{
						sphere2.SetTarget(null);
					}
					UnityEngine.Object.Destroy(tempObject2);
				}
				if (launchSite.isPQSCity2)
				{
					GClass4 sphere2 = launchSite.pqsCity2.sphere;
					Planetarium.CelestialFrame cf2 = default(Planetarium.CelestialFrame);
					Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf2);
					Vector3d vector3d2 = LatLon.GetSurfaceNVector(cf2, launchSite.pqsCity2.lat, launchSite.pqsCity2.lon) * (sphere2.radius + launchSite.pqsCity2.alt);
					GameObject tempObject2 = new GameObject("TempLocation_" + launchSite.pqsCity2.objectName);
					tempObject2.transform.SetParent(sphere2.transform);
					tempObject2.transform.localPosition = vector3d2;
					FloatingOrigin.SetOffset(tempObject2.transform.position);
					yield return new WaitForFixedUpdate();
					launchSite.pqsCity2.Reset();
					if (launchSite.positionMobileLaunchPad != null)
					{
						launchSite.positionMobileLaunchPad.ResetPositioning();
						launchSite.positionMobileLaunchPad.launchSite = launchSite;
					}
					if (forcePQSPosition)
					{
						ForceInitPQS(sphere2, tempObject2.transform);
						yield return new WaitForSeconds(2f);
					}
					launchSite.pqsCity2.Orientate();
					if (forcePQSPosition)
					{
						sphere2.SetTarget(null);
					}
					UnityEngine.Object.Destroy(tempObject2);
				}
			}
			else
			{
				Debug.Log("PSystemSetup: Cannot add Stock launchsite named '" + stocklaunchsites[lsI].launchSiteName + "' - Launchsite is not setup.");
				yield return null;
			}
		}
		for (int i = 0; i < spherestoSetup.Count; i++)
		{
			spherestoSetup[i].ResetModList();
		}
		yield return null;
	}

	public void SpawnAdditionalPrefabs(bool setupSuccess, LaunchSite launchSite, GameObject PQSParent)
	{
		if (!setupSuccess || launchSite.additionalprefabPaths.Length == 0 || string.IsNullOrEmpty(launchSite.BundleName))
		{
			return;
		}
		for (int i = 0; i < launchSite.additionalprefabPaths.Length; i++)
		{
			GameObject gameObject = BundleLoader.LoadAsset<PQSCity2>(launchSite.BundleName, launchSite.additionalprefabPaths[i]) as GameObject;
			if (gameObject == null && !string.IsNullOrEmpty(launchSite.additionalprefabPaths[i]))
			{
				string[] array = launchSite.additionalprefabPaths[i].Split('/');
				if (array != null && array.Length != 0)
				{
					gameObject = AssetBase.GetPrefab(array[array.Length - 1]);
				}
			}
			if (!(gameObject != null) || !(PQSParent != null))
			{
				continue;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, PQSParent.transform);
			if (gameObject2 != null)
			{
				int num = gameObject2.name.IndexOf("(Clone)", StringComparison.InvariantCultureIgnoreCase);
				if (num != -1)
				{
					gameObject2.name = gameObject2.name.Remove(num);
				}
				PQSCity component = gameObject2.GetComponent<PQSCity>();
				PQSCity2 component2 = gameObject2.GetComponent<PQSCity2>();
				if (component != null)
				{
					component.sphere = PQSParent.GetComponent<GClass4>();
				}
				if (component2 != null)
				{
					component2.sphere = PQSParent.GetComponent<GClass4>();
				}
			}
		}
	}

	public GameObject FindPQS(string pqsName)
	{
		int num = 0;
		while (true)
		{
			if (num < pqsArray.Length)
			{
				if (pqsArray[num].gameObject.name == pqsName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return pqsArray[num].gameObject;
	}

	public bool IsStockLaunchSite(string name)
	{
		int num = 0;
		while (true)
		{
			if (num < stocklaunchsites.Length)
			{
				if (stocklaunchsites[num].name == name || stocklaunchsites[num].launchSiteName == name)
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

	public bool IsFacilityOrLaunchSite(string inputName, out bool isFacility, out string displayName)
	{
		isFacility = false;
		displayName = "";
		if (IsFacility(inputName, out displayName))
		{
			isFacility = true;
			return true;
		}
		if (IsLaunchSite(inputName, out displayName))
		{
			return true;
		}
		return false;
	}

	public bool IsFacility(string inputName, out string displayName)
	{
		displayName = "";
		int num = 0;
		while (true)
		{
			if (num < facilities.Length)
			{
				if (facilities[num].name == inputName || facilities[num].facilityName == inputName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		displayName = facilities[num].facilityDisplayName;
		return true;
	}

	public bool IsLaunchSite(string inputName, out string displayName)
	{
		displayName = "";
		int num = 0;
		while (true)
		{
			if (num < launchsites.Count)
			{
				if (launchsites[num].name == inputName || launchsites[num].launchSiteName == inputName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		displayName = launchsites[num].launchSiteName;
		return true;
	}

	public void RemoveNonStockLaunchSites()
	{
	}

	public LaunchSite GetLaunchSite(string name)
	{
		int num = 0;
		while (true)
		{
			if (num < launchsites.Count)
			{
				if (launchsites[num].name == name || launchsites[num].launchSiteName == name)
				{
					if (string.Equals(launchsites[num].BundleName, "makinghistory_assets") && ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
					{
						return launchsites[num];
					}
					if (string.Equals(launchsites[num].BundleName, string.Empty) || string.Equals(launchsites[num].BundleName, "stock"))
					{
						break;
					}
				}
				num++;
				continue;
			}
			return null;
		}
		return launchsites[num];
	}

	public CelestialBody GetLaunchSiteBody(string name)
	{
		SpaceCenterFacility spaceCenterFacility = GetSpaceCenterFacility(name);
		if (spaceCenterFacility != null)
		{
			return spaceCenterFacility.facilityPQS.GetComponentInParent<CelestialBody>();
		}
		return GetLaunchSite(name)?.launchsitePQS.GetComponentInParent<CelestialBody>();
	}

	public string GetLaunchSiteBodyName(string name)
	{
		SpaceCenterFacility spaceCenterFacility = GetSpaceCenterFacility(name);
		if (spaceCenterFacility != null)
		{
			return spaceCenterFacility.pqsName;
		}
		return GetLaunchSite(name)?.pqsName;
	}

	public string GetLaunchSiteDisplayName(string name)
	{
		SpaceCenterFacility spaceCenterFacility = GetSpaceCenterFacility(name);
		if (spaceCenterFacility != null)
		{
			return Localizer.Format(spaceCenterFacility.facilityDisplayName);
		}
		LaunchSite launchSite = GetLaunchSite(name);
		if (launchSite != null)
		{
			return Localizer.Format(launchSite.launchSiteName);
		}
		return name;
	}

	public bool AddLaunchSite(LaunchSite launchsite)
	{
		return AddLaunchSite(launchsite, overRide: false);
	}

	public bool AddLaunchSite(LaunchSite launchsite, bool overRide)
	{
		if (GetLaunchSite(launchsite.launchSiteName) == null)
		{
			if (!overRide && !launchsite.IsSetup)
			{
				Debug.Log("PSystemSetup: Cannot add launchsite named '" + launchsite.launchSiteName + "' - Launchsite is not setup.");
				return false;
			}
			launchsites.Add(launchsite);
			if (launchsite.pqsCity != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(launchsite.pqsCity.gameObject);
			}
			if (launchsite.pqsCity2 != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(launchsite.pqsCity2.gameObject);
			}
			return true;
		}
		Debug.Log("PSystemSetup: Cannot add launchsite named '" + launchsite.launchSiteName + "' - Already exists.");
		return false;
	}

	public bool RemoveLaunchSite(string name)
	{
		LaunchSite launchSite = GetLaunchSite(name);
		if (launchSite != null)
		{
			if (launchSite.pqsCity != null)
			{
				launchSite.pqsCity.OnSphereInactive();
				launchSite.pqsCity.gameObject.DestroyGameObject();
			}
			if (launchSite.pqsCity2 != null)
			{
				launchSite.pqsCity2.OnSphereInactive();
				launchSite.pqsCity2.gameObject.DestroyGameObject();
			}
			launchsites.Remove(launchSite);
			return true;
		}
		Debug.Log("PSystemSetup: Cannot remove launchsite named '" + name + "' - Launchsite does not exist.");
		return false;
	}

	public SpaceCenterFacility GetSpaceCenterFacility(string name)
	{
		SpaceCenterFacility[] array = facilities;
		int num = 0;
		SpaceCenterFacility spaceCenterFacility;
		while (true)
		{
			if (num < array.Length)
			{
				spaceCenterFacility = array[num];
				if (spaceCenterFacility.name == name || spaceCenterFacility.facilityName == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return spaceCenterFacility;
	}

	public void LoadTestScene(GameScenes sceneBase)
	{
		loadTestScene = true;
		nextScene = sceneBase;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes level)
	{
		switch (level)
		{
		case GameScenes.MISSIONBUILDER:
			SetMissionEditor();
			break;
		case GameScenes.LOADINGBUFFER:
			SetDisabled();
			break;
		case GameScenes.MAINMENU:
			SetMainMenu();
			break;
		default:
			SetDisabled();
			break;
		case GameScenes.SPACECENTER:
			SetSpaceCentre();
			break;
		case GameScenes.EDITOR:
			SetEditor();
			break;
		case GameScenes.FLIGHT:
			SetFlight();
			break;
		case GameScenes.TRACKSTATION:
			SetTrackingStation();
			break;
		case GameScenes.PSYSTEM:
			break;
		}
	}

	public void OnSceneChange(GameScenes scene)
	{
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			celestialBody.inverseRotation = false;
			if (celestialBody.orbitDriver != null)
			{
				celestialBody.orbitDriver.reverse = false;
			}
			celestialBody.SetupShadowCasting();
		}
		ResetTransforms();
		SetDisabled();
	}

	public void SetMainMenu()
	{
		Planetarium.fetch.pause = true;
		PlanetariumCamera.fetch.Deactivate();
		ScaledCamera.Instance.enabled = false;
		GalaxyCubeControl.Instance.SetEnabled(state: false);
		FlightCamera.fetch.DisableCamera();
		FlightCamera.fetch.enabled = false;
		if (Sun.Instance != null)
		{
			Sun.Instance.SunlightEnabled(state: false);
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.SunlightEnabled(state: false);
		}
		DynamicAmbientLight.Instance.disableDynamicAmbient = true;
		cb.forceActivate = false;
		ScaledSpace.Instance.gameObject.SetActive(value: false);
		FloatingOrigin.SetOffset(scTransform.position);
		SetPQSInactive();
	}

	public void SetSpaceCentre()
	{
		Planetarium.fetch.pause = false;
		Planetarium.fetch.timeScale = 1.0;
		PlanetariumCamera.fetch.Deactivate();
		ScaledCamera.Instance.enabled = true;
		GalaxyCubeControl.Instance.SetEnabled(state: true);
		FlightCamera.fetch.EnableCamera();
		FlightCamera.fetch.SetDefaultFoV();
		FlightCamera.fetch.enabled = false;
		if (Sun.Instance != null)
		{
			Sun.Instance.SunlightEnabled(state: true);
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.SunlightEnabled(state: true);
		}
		DynamicAmbientLight.Instance.disableDynamicAmbient = true;
		cb.forceActivate = true;
		cb.sphere.SetTarget(FlightCamera.fetch.transform);
		FlightGlobals.currentMainBody = cb.body;
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			celestialBody.inverseRotation = false;
			if (celestialBody.orbitDriver != null)
			{
				celestialBody.orbitDriver.reverse = false;
			}
		}
		cb.body.inverseRotation = true;
		if (cb.body.orbitDriver != null)
		{
			cb.body.orbitDriver.reverse = true;
		}
		ScaledSpace.Instance.gameObject.SetActive(value: true);
		SetPQSActive(pqs);
		FloatingOrigin.SetOffset(scTransform.position);
	}

	public void SetMissionEditor()
	{
		SetMainMenu();
		ScaledCamera.Instance.enabled = true;
		ScaledSpace.Instance.gameObject.SetActive(value: true);
	}

	public void SetFlight()
	{
		Planetarium.fetch.pause = false;
		PlanetariumCamera.fetch.Deactivate();
		ScaledCamera.Instance.enabled = true;
		GalaxyCubeControl.Instance.SetEnabled(state: true);
		FlightCamera.fetch.EnableCamera();
		FlightCamera.fetch.enabled = true;
		if (Sun.Instance != null)
		{
			Sun.Instance.SunlightEnabled(state: true);
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.SunlightEnabled(state: true);
		}
		DynamicAmbientLight.Instance.disableDynamicAmbient = false;
		cb.forceActivate = false;
		ScaledSpace.Instance.gameObject.SetActive(value: true);
	}

	public void SetTrackingStation()
	{
		Planetarium.fetch.pause = false;
		Planetarium.fetch.timeScale = 1.0;
		PlanetariumCamera.fetch.Activate();
		ScaledCamera.Instance.enabled = false;
		GalaxyCubeControl.Instance.SetEnabled(state: true);
		FlightCamera.fetch.DisableCamera();
		FlightCamera.fetch.enabled = false;
		if (Sun.Instance != null)
		{
			Sun.Instance.SunlightEnabled(state: true);
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.SunlightEnabled(state: true);
		}
		DynamicAmbientLight.Instance.disableDynamicAmbient = true;
		cb.forceActivate = false;
		ScaledSpace.Instance.gameObject.SetActive(value: true);
		FloatingOrigin.SetOffset(scTransform.position);
	}

	public void SetEditor()
	{
		Planetarium.fetch.pause = true;
		PlanetariumCamera.fetch.Deactivate();
		ScaledCamera.Instance.enabled = false;
		GalaxyCubeControl.Instance.SetEnabled(state: false);
		FlightCamera.fetch.DisableCamera();
		FlightCamera.fetch.enabled = false;
		if (Sun.Instance != null)
		{
			Sun.Instance.SunlightEnabled(state: false);
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.SunlightEnabled(state: false);
		}
		DynamicAmbientLight.Instance.disableDynamicAmbient = true;
		cb.forceActivate = false;
		ScaledSpace.Instance.gameObject.SetActive(value: false);
		FloatingOrigin.SetOffset(scTransform.position);
		if (!pqs.isStarted)
		{
			ForceInitPQS();
		}
		SetPQSInactive();
	}

	public void SetDisabled()
	{
		Planetarium.fetch.pause = true;
		PlanetariumCamera.fetch.Deactivate();
		ScaledCamera.Instance.enabled = false;
		GalaxyCubeControl.Instance.SetEnabled(state: false);
		FlightCamera.fetch.DisableCamera();
		FlightCamera.fetch.enabled = false;
		Sun.Instance.SunlightEnabled(state: false);
		FlightGlobals.currentMainBody = null;
		DynamicAmbientLight.Instance.disableDynamicAmbient = true;
		cb.forceActivate = false;
		ScaledSpace.Instance.gameObject.SetActive(value: false);
		SetPQSDisabled();
	}

	public void ForceInitPQS()
	{
		cb.forceActivate = true;
		cb.sphere.SetTarget(FlightCamera.fetch.transform);
		SetPQSActive(pqs);
	}

	public void ForceInitPQS(GClass4 pqsIn, Transform target = null)
	{
		if (pqsIn.PQSModCBTransform != null)
		{
			if (target != null)
			{
				pqsIn.SetTarget(target);
			}
			else
			{
				pqsIn.SetTarget(FlightCamera.fetch.transform);
			}
		}
		pqsIn.ForceStart();
	}

	public void SetPQSInactive(bool keepActiveInFlight = false)
	{
		GClass4[] array = pqsArray;
		foreach (GClass4 gClass in array)
		{
			if (keepActiveInFlight && FlightGlobals.currentMainBody != null)
			{
				CelestialBody componentInParent = gClass.transform.GetComponentInParent<CelestialBody>();
				if (componentInParent != null && (componentInParent == FlightGlobals.currentMainBody || (HighLogic.LoadedScene == GameScenes.SPACECENTER && componentInParent == FlightGlobals.GetHomeBody())))
				{
					continue;
				}
			}
			gClass.isDisabled = true;
			gClass.ResetSphere();
		}
	}

	public void SetPQSActive(GClass4 pqs)
	{
		GClass4[] array = pqsArray;
		foreach (GClass4 gClass in array)
		{
			gClass.isDisabled = false;
			if (gClass != pqs)
			{
				gClass.ResetAndWait();
			}
			else
			{
				gClass.ForceStart();
			}
		}
	}

	public void SetPQSActive()
	{
		GClass4[] array = pqsArray;
		foreach (GClass4 obj in array)
		{
			obj.isDisabled = false;
			obj.ResetAndWait();
		}
	}

	public void SetPQSDisabled()
	{
		GClass4[] array = pqsArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].isDisabled = true;
		}
	}

	public void ResetTransforms()
	{
		Planetarium.fetch.pause = true;
		FlightCamera.fetch.SetFoV(60f);
		ScaledCamera.Instance.SetFoV(60f);
		FlightCamera.fetch.transform.parent = base.transform;
		PlanetariumCamera.fetch.Deactivate();
	}
}
