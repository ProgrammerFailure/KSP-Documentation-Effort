using System;
using System.Collections.Generic;
using ns2;
using UnityEngine;

namespace ns11;

public class KnowledgeBase : MonoBehaviour
{
	public enum KbTargetType
	{
		Null,
		Vessel,
		CelestialBody,
		Unowned
	}

	public ApplicationLauncherButton appLauncherButtonPrefab;

	public KbApp[] appPrefabs;

	public bool showing;

	public UIList applauncherList;

	public List<KbApp> apps = new List<KbApp>();

	public static KnowledgeBase Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("KnowledgeBase already exist, destroying this instance");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		GameEvents.onGUIApplicationLauncherReady.Add(OnAppLauncherReady);
		GameEvents.onGameSceneSwitchRequested.Add(Cleanup);
		MapView.OnEnterMapView = (Callback)Delegate.Combine(MapView.OnEnterMapView, new Callback(Show));
		MapView.OnExitMapView = (Callback)Delegate.Combine(MapView.OnExitMapView, new Callback(Hide));
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Cleanup(GameEvents.FromToAction<GameScenes, GameScenes> action)
	{
		GameEvents.onGUIApplicationLauncherReady.Remove(OnAppLauncherReady);
		GameEvents.onGameSceneSwitchRequested.Remove(Cleanup);
		GameEvents.onPlanetariumTargetChanged.Remove(OnMapFocusChange);
		MapView.OnEnterMapView = (Callback)Delegate.Remove(MapView.OnEnterMapView, new Callback(Show));
		MapView.OnExitMapView = (Callback)Delegate.Remove(MapView.OnExitMapView, new Callback(Hide));
		int i = 0;
		for (int count = apps.Count; i < count; i++)
		{
			if (apps[i].appLauncherButton != null)
			{
				apps[i].appLauncherButton.transform.SetParent(null);
				UnityEngine.Object.Destroy(apps[i].appLauncherButton);
			}
		}
		if (applauncherList != null && applauncherList.transform.parent != null)
		{
			applauncherList.transform.parent.gameObject.SetActive(value: false);
		}
	}

	public void OnAppLauncherReady()
	{
		Debug.Log("[KnowledgeBase] OnAppLauncherReady " + Time.frameCount);
		applauncherList = ApplicationLauncher.Instance.GetKnowledgeBaseList();
		GameEvents.onPlanetariumTargetChanged.Add(OnMapFocusChange);
		int i = 0;
		for (int num = appPrefabs.Length; i < num; i++)
		{
			KbApp kbApp = UnityEngine.Object.Instantiate(appPrefabs[i]);
			kbApp.Setup();
			apps.Add(kbApp);
			kbApp.appLauncherButton.transform.SetParent(applauncherList.transform, worldPositionStays: false);
			kbApp.appLauncherButton.gameObject.SetActive(value: false);
			kbApp.appLauncherButton.VisibleInScenes = apps[i].appLauncherButton.VisibleInScenes & ~ApplicationLauncher.AppScenes.FLIGHT;
		}
	}

	public void Show()
	{
		if (applauncherList != null)
		{
			applauncherList.transform.parent.gameObject.SetActive(value: true);
		}
		showing = true;
		int i = 0;
		for (int count = apps.Count; i < count; i++)
		{
			apps[i].Restore();
		}
	}

	public void Hide()
	{
		if (applauncherList != null)
		{
			applauncherList.transform.parent.gameObject.SetActive(value: false);
		}
		showing = false;
		int i = 0;
		for (int count = apps.Count; i < count; i++)
		{
			apps[i].Hide();
		}
	}

	public void ActivateApps(KbTargetType targetType, MapObject target)
	{
		bool flag = false;
		KbApp appToOpen = null;
		int i = 0;
		for (int count = apps.Count; i < count; i++)
		{
			if (apps[i].appIsLive && apps[i].appName.Contains("Info") && apps[i].targetType != targetType)
			{
				flag = true;
				break;
			}
		}
		int j = 0;
		for (int count2 = apps.Count; j < count2; j++)
		{
			if (apps[j].targetType == targetType)
			{
				apps[j].ActivateApp(target);
				apps[j].appLauncherButton.gameObject.SetActive(value: true);
				if (flag && apps[j].appName.Contains("Info"))
				{
					appToOpen = apps[j];
				}
				continue;
			}
			if (apps[j].appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.True)
			{
				apps[j].appLauncherButton.toggleButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.USER, null);
				apps[j].appLauncherButton.onLeftClickBtn(apps[j].appLauncherButton.toggleButton);
			}
			if (apps[j].appLauncherButton != null)
			{
				apps[j].appLauncherButton.gameObject.SetActive(value: false);
			}
		}
		if (appToOpen != null)
		{
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				appToOpen.appLauncherButton.toggleButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.USER, null);
				appToOpen.appLauncherButton.onLeftClickBtn(appToOpen.appLauncherButton.toggleButton);
			}));
		}
	}

	public void OnMapFocusChange(MapObject target)
	{
		if (target == null)
		{
			Hide();
			ActivateApps(KbTargetType.Null, target);
		}
		else if ((target.vessel != null && target.vessel.DiscoveryInfo.Level != DiscoveryLevels.Owned) || (target.celestialBody != null && target.celestialBody.DiscoveryInfo.Level != DiscoveryLevels.Owned))
		{
			if (!showing)
			{
				Show();
			}
			ActivateApps(KbTargetType.Unowned, target);
		}
		else if (target.type == MapObject.ObjectType.CelestialBody)
		{
			if (!showing)
			{
				Show();
			}
			ActivateApps(KbTargetType.CelestialBody, target);
		}
		else if (target.type == MapObject.ObjectType.Vessel)
		{
			if (!showing)
			{
				Show();
			}
			ActivateApps(KbTargetType.Vessel, target);
		}
		if (!MapView.MapIsEnabled)
		{
			Hide();
		}
	}

	public static double GetUnloadedVesselMass(Vessel v)
	{
		double num = 0.0;
		int count = v.protoVessel.protoPartSnapshots.Count;
		while (count-- > 0)
		{
			ProtoPartSnapshot protoPartSnapshot = v.protoVessel.protoPartSnapshots[count];
			num += (double)protoPartSnapshot.mass;
			int count2 = protoPartSnapshot.resources.Count;
			while (count2-- > 0)
			{
				ProtoPartResourceSnapshot protoPartResourceSnapshot = protoPartSnapshot.resources[count2];
				if (protoPartResourceSnapshot != null && protoPartResourceSnapshot.definition != null)
				{
					num += (double)protoPartResourceSnapshot.definition.density * protoPartResourceSnapshot.amount;
				}
			}
		}
		return num;
	}

	public static List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>> GetVesselCrewByAvailablePart(Vessel v)
	{
		List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>> list = new List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>>();
		if (v.loaded)
		{
			int i = 0;
			for (int count = v.parts.Count; i < count; i++)
			{
				Part part = v.parts[i];
				if (part.protoModuleCrew.Count != 0)
				{
					list.Add(new KeyValuePair<AvailablePart, List<ProtoCrewMember>>(part.partInfo, part.protoModuleCrew));
				}
			}
		}
		else
		{
			int count2 = v.protoVessel.protoPartSnapshots.Count;
			for (int j = 0; j < count2; j++)
			{
				ProtoPartSnapshot protoPartSnapshot = v.protoVessel.protoPartSnapshots[j];
				if (protoPartSnapshot.protoModuleCrew.Count != 0)
				{
					list.Add(new KeyValuePair<AvailablePart, List<ProtoCrewMember>>(protoPartSnapshot.partInfo, protoPartSnapshot.protoModuleCrew));
				}
			}
		}
		return list;
	}
}
