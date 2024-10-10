using ns2;
using UnityEngine;

namespace ns11;

public class VSDSceneSpawner : MonoBehaviour
{
	public UICanvasPrefab VSDScreenPrefab;

	public void Awake()
	{
		GameEvents.onGUILaunchScreenSpawn.Add(onVSDSpawn);
		GameEvents.onGUILaunchScreenDespawn.Add(onVSDDespawn);
	}

	public void OnDestroy()
	{
		GameEvents.onGUILaunchScreenSpawn.Remove(onVSDSpawn);
		GameEvents.onGUILaunchScreenDespawn.Remove(onVSDDespawn);
	}

	public void onVSDSpawn(GameEvents.VesselSpawnInfo info)
	{
		EditorDriver.editorFacility = info.callingFacility.facilityType;
		EditorDriver.setupValidLaunchSites();
		UIMasterController.Instance.AddCanvas(VSDScreenPrefab);
		StartCoroutine(CallbackUtil.DelayedCallback(1, VesselSpawnDialog.Instance.InitiateGUI, info));
	}

	public void onVSDDespawn()
	{
		UIMasterController.Instance.RemoveCanvas(VSDScreenPrefab);
	}
}
