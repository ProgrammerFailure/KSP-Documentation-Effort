using System;
using System.Collections;
using CommNet;
using Expansions.Missions.Editor;
using Expansions.Missions.Scenery.Scripts;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class LaunchSiteSituation : IConfigNode
{
	public string launchSiteName = Localizer.Format("#autoLOC_8100066");

	[MEGUI_Dropdown(addDefaultOption = false, canBePinned = true, guiName = "#autoLOC_8100067")]
	public EditorFacility facility = EditorFacility.const_1;

	[MEGUI_Checkbox(canBePinned = true, canBeReset = true, guiName = "#autoLOC_8100068")]
	public bool showRamp;

	[MEGUI_VesselGroundLocation(DisableRotationY = true, DisableRotationX = true, AllowWaterSurfacePlacement = false, gapDisplay = true, guiName = "#autoLOC_8100069", Tooltip = "#autoLOC_8100070")]
	public VesselGroundLocation launchSiteGroundLocation;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8002007", Tooltip = "#autoLOC_8002111")]
	public bool splashed;

	public PQSCity2 pqsCity2;

	public LaunchSite launchSite;

	public GameObject launchSiteObject;

	public string launchSiteObjectName;

	public string LaunchSiteObjectName
	{
		get
		{
			launchSiteObjectName = KSPUtil.SanitizeString(launchSiteName, '_', replaceEmpty: false);
			launchSiteObjectName = launchSiteObjectName.Replace(' ', '_');
			return launchSiteObjectName;
		}
	}

	public LaunchSiteSituation(MENode node)
	{
		launchSiteGroundLocation = new VesselGroundLocation(node, VesselGroundLocation.GizmoIcon.LaunchSite);
	}

	public IEnumerator AddLaunchSite(bool createObject)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && PSystemSetup.Instance != null && PSystemSetup.Instance.mobileLaunchSitePrefab != null)
		{
			if (launchSiteObject == null)
			{
				LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(LaunchSiteObjectName);
				if (launchSite != null)
				{
					if (!(launchSite.pqsCity2 == null) && (!(launchSite.pqsCity2 != null) || !(launchSite.pqsCity2.celestialBody != launchSiteGroundLocation.targetBody)))
					{
						if (HighLogic.LoadedSceneIsMissionBuilder)
						{
							ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8100287", launchSiteName));
						}
						Debug.Log("A LaunchSite named '" + launchSiteName + "' already exists.");
						yield break;
					}
					PSystemSetup.Instance.RemoveLaunchSite(LaunchSiteObjectName);
				}
				if (createObject)
				{
					CoroutineHost coroutineHost = CoroutineHost.Create("GenerateLaunchSiteObject", persistThroughSceneChanges: false, disposable: true);
					yield return coroutineHost.StartCoroutine(createLaunchSiteObject());
				}
				else
				{
					createEmptyLaunchSite();
				}
			}
			else
			{
				pqsCity2 = launchSiteObject.GetComponent<PQSCity2>();
				if (pqsCity2 != null && pqsCity2.celestialBody != launchSiteGroundLocation.targetBody)
				{
					PSystemSetup.Instance.RemoveLaunchSite(LaunchSiteObjectName);
					if (createObject)
					{
						CoroutineHost coroutineHost2 = CoroutineHost.Create("GenerateLaunchSiteObject", persistThroughSceneChanges: false, disposable: true);
						yield return coroutineHost2.StartCoroutine(createLaunchSiteObject());
					}
					else
					{
						createEmptyLaunchSite();
					}
				}
			}
		}
		yield return null;
	}

	public IEnumerator createLaunchSiteObject()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			launchSiteObject = UnityEngine.Object.Instantiate(PSystemSetup.Instance.mobileLaunchSitePrefab);
			if (launchSiteObject != null)
			{
				PositionMobileLaunchPad component = launchSiteObject.GetComponent<PositionMobileLaunchPad>();
				if (component != null)
				{
					component.hideRampOverMax = !showRamp;
				}
				pqsCity2 = launchSiteObject.GetComponent<PQSCity2>();
				launchSiteObject.name = LaunchSiteObjectName;
				if (pqsCity2 != null)
				{
					pqsCity2.objectName = LaunchSiteObjectName;
					pqsCity2.displayobjectName = launchSiteName;
					if ((bool)pqsCity2.crashObjectName)
					{
						pqsCity2.crashObjectName.objectName = pqsCity2.objectName;
						pqsCity2.crashObjectName.displayName = Localizer.Format(pqsCity2.displayobjectName);
					}
					pqsCity2.lat = launchSiteGroundLocation.latitude;
					pqsCity2.lon = launchSiteGroundLocation.longitude;
					for (int i = 0; i < PSystemSetup.Instance.pqsArray.Length; i++)
					{
						if (PSystemSetup.Instance.pqsArray[i].gameObject.name == launchSiteGroundLocation.targetBody.bodyName)
						{
							pqsCity2.transform.SetParent(PSystemSetup.Instance.pqsArray[i].gameObject.transform);
							pqsCity2.sphere = PSystemSetup.Instance.pqsArray[i];
							break;
						}
					}
					Vector3 eulerAngles = launchSiteGroundLocation.rotation.eulerAngles;
					pqsCity2.rotation = eulerAngles.z;
					LaunchSite.SpawnPoint spawnPoint = new LaunchSite.SpawnPoint();
					spawnPoint.name = LaunchSiteObjectName;
					spawnPoint.spawnTransformURL = "SpawnPoint";
					launchSite = new LaunchSite(spawnPoints: new LaunchSite.SpawnPoint[1] { spawnPoint }, name: LaunchSiteObjectName, pqsName: launchSiteGroundLocation.targetBody.bodyName, launchSiteName: launchSiteName, launchSiteTransformURL: LaunchSiteObjectName, editorFacility: facility);
					if (launchSite != null)
					{
						if (launchSite.Setup(pqsCity2, PSystemSetup.Instance.pqsArray))
						{
							PSystemSetup.Instance.AddLaunchSite(launchSite);
						}
						pqsCity2.launchSite = launchSite;
					}
					yield return null;
					pqsCity2.SetBody();
					if (pqsCity2.celestialBody != null)
					{
						Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
						Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
						Vector3d vector3d = LatLon.GetSurfaceNVector(cf, pqsCity2.lat, pqsCity2.lon) * (pqsCity2.sphere.radius + pqsCity2.alt);
						pqsCity2.transform.localPosition = vector3d;
						pqsCity2.setOnWaterSurface = splashed;
						pqsCity2.Reset();
						if (launchSite.positionMobileLaunchPad != null)
						{
							launchSite.positionMobileLaunchPad.ResetPositioning();
							launchSite.positionMobileLaunchPad.launchSite = launchSite;
						}
						pqsCity2.Orientate();
						yield return null;
					}
				}
				CommNetHome component2 = launchSiteObject.GetComponent<CommNetHome>();
				if (component2 != null)
				{
					component2.displaynodeName = launchSiteName;
					component2.nodeName = launchSiteGroundLocation.targetBody.bodyName + ": " + LaunchSiteObjectName;
				}
			}
		}
		yield return null;
	}

	public void createEmptyLaunchSite()
	{
		launchSite = new LaunchSite(LaunchSiteObjectName, launchSiteGroundLocation.targetBody.bodyName, launchSiteName, null, LaunchSiteObjectName, facility);
		if (launchSite == null)
		{
			return;
		}
		for (int i = 0; i < PSystemSetup.Instance.pqsArray.Length; i++)
		{
			if (PSystemSetup.Instance.pqsArray[i].gameObject.name == launchSite.pqsName)
			{
				launchSite.launchsitePQS = PSystemSetup.Instance.pqsArray[i];
				break;
			}
		}
		PSystemSetup.Instance.AddLaunchSite(launchSite, overRide: true);
	}

	public void RemoveLaunchSite()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && PSystemSetup.Instance != null && PSystemSetup.Instance.GetLaunchSite(LaunchSiteObjectName) != null)
		{
			PSystemSetup.Instance.RemoveLaunchSite(LaunchSiteObjectName);
		}
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "splashed":
				splashed = bool.Parse(value.value);
				break;
			case "showRamp":
				showRamp = bool.Parse(value.value);
				break;
			case "facility":
				node.TryGetEnum("facility", ref facility, EditorFacility.None);
				break;
			case "launchSiteName":
				launchSiteName = value.value;
				break;
			}
		}
		for (int j = 0; j < node.nodes.Count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			string name = configNode.name;
			if (!(name == "GROUNDPOINT"))
			{
				continue;
			}
			launchSiteGroundLocation.Load(configNode);
			if (!node.HasValue("alignedNorth"))
			{
				launchSiteGroundLocation.rotation.eulerZ += 90f;
				if (launchSiteGroundLocation.rotation.eulerZ > 360f)
				{
					launchSiteGroundLocation.rotation.eulerZ -= 360f;
				}
			}
		}
	}

	public void Save(ConfigNode node)
	{
		ConfigNode configNode = node.AddNode("LAUNCHSITESITUATION");
		configNode.AddValue("launchSiteName", launchSiteName);
		configNode.AddValue("facility", facility.ToString());
		configNode.AddValue("showRamp", showRamp);
		configNode.AddValue("splashed", splashed);
		if (launchSiteGroundLocation != null)
		{
			launchSiteGroundLocation.Save(configNode.AddNode("GROUNDPOINT"));
		}
		configNode.AddValue("alignedNorth", value: true);
	}
}
