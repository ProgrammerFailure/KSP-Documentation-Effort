using UnityEngine;

public class SpaceCenterSurfaceObject : MonoBehaviour
{
	public SurfaceObject srfObj;

	public CelestialBody cb;

	public bool setup;

	public void Awake()
	{
		GameEvents.onLevelWasLoaded.Add(OnGameSceneLoaded);
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
	}

	public void Setup()
	{
		cb = base.gameObject.GetComponentUpwards<CelestialBody>();
		srfObj = SurfaceObject.Create(base.gameObject, cb, 5, KFSMUpdateMode.FIXEDUPDATE);
		setup = true;
	}

	public void OnGameSceneLoaded(GameScenes scn)
	{
		if (scn == GameScenes.SPACECENTER || scn == GameScenes.FLIGHT)
		{
			if (!setup)
			{
				Setup();
			}
			else
			{
				HighLogic.fetch.StartCoroutine(CallbackUtil.DelayedCallback(5, srfObj.PopToSceneRoot));
			}
		}
	}

	public void OnGameSceneLoadRequested(GameScenes scn)
	{
		GameScenes loadedScene = HighLogic.LoadedScene;
		if ((loadedScene == GameScenes.SPACECENTER || loadedScene == GameScenes.FLIGHT) && setup)
		{
			srfObj.ReturnToParent();
		}
	}

	public void OnDestroy()
	{
		if (setup && srfObj.IsPopped)
		{
			srfObj.ReturnToParent();
		}
		GameEvents.onLevelWasLoaded.Remove(OnGameSceneLoaded);
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
	}
}
