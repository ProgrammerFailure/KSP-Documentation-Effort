using System.Collections;
using UnityEngine;

namespace TestScripts;

public class SceneryTestSceneLoader : MonoBehaviour
{
	public int postInitDelay = 3;

	public GameObject[] activateOnPostInit;

	public bool mapModeEnabled;

	public GameObject mapViewPrefab;

	public SpaceCenterCamera2 kscCamera;

	public float zoomSpeedAtMin = 300f;

	public float zoomSpeedAtMax = 30000f;

	public GameScenes loadAsScene = GameScenes.SPACECENTER;

	public void Awake()
	{
		int num = activateOnPostInit.Length;
		while (num-- > 0)
		{
			activateOnPostInit[num].SetActive(value: false);
		}
	}

	public IEnumerator Start()
	{
		GameDatabase.Instance.LoadEmpty();
		PSystemSetup.Instance.LoadTestScene(loadAsScene);
		HighLogic.CurrentGame = new Game();
		for (int i = 0; i < postInitDelay; i++)
		{
			yield return null;
		}
		for (int j = 0; j < activateOnPostInit.Length; j++)
		{
			activateOnPostInit[j].SetActive(value: true);
		}
		Object.Instantiate(mapViewPrefab);
		MapView.fetch.StartCoroutine(MapView.fetch.Start());
		QualitySettings.shadowDistance = 5000f;
	}

	public void Update()
	{
		if (GameSettings.MAP_VIEW_TOGGLE.GetKeyDown())
		{
			MapView.MapIsEnabled = !MapView.MapIsEnabled;
			if (MapView.MapIsEnabled)
			{
				MapView.fetch.VectorCamera.enabled = true;
				PlanetariumCamera.fetch.Activate(enableAudioListener: false);
				FlightCamera.fetch.DisableCamera(disableAudioListener: false);
				ScaledCamera.Instance.enabled = false;
				kscCamera.enabled = false;
				PlanetariumCamera.fetch.SetTarget(PlanetariumCamera.fetch.target);
				for (int i = 0; i < Planetarium.Orbits.Count; i++)
				{
					OrbitDriver orbitDriver = Planetarium.Orbits[i];
					if (orbitDriver.Renderer != null)
					{
						if ((bool)orbitDriver.vessel && orbitDriver.vessel.PatchedConicsAttached)
						{
							orbitDriver.Renderer.drawMode = OrbitRendererBase.DrawMode.const_0;
						}
						else
						{
							orbitDriver.Renderer.drawMode = OrbitRendererBase.DrawMode.REDRAW_AND_RECALCULATE;
						}
						orbitDriver.Renderer.drawNodes = true;
					}
				}
			}
			else
			{
				MapView.fetch.VectorCamera.enabled = false;
				foreach (OrbitDriver orbit in Planetarium.Orbits)
				{
					if (orbit.Renderer != null)
					{
						orbit.Renderer.drawMode = OrbitRendererBase.DrawMode.const_0;
						orbit.Renderer.drawNodes = false;
					}
				}
				PlanetariumCamera.fetch.Deactivate();
				FlightCamera.fetch.EnableCamera();
				FlightCamera.fetch.enabled = false;
				ScaledCamera.Instance.enabled = true;
				kscCamera.enabled = true;
			}
		}
		kscCamera.zoomSpeed = Mathf.Lerp(zoomSpeedAtMin, zoomSpeedAtMax, Mathf.InverseLerp(kscCamera.zoomMin, kscCamera.zoomMax, kscCamera.GetZoom()));
		if ((bool)MapView.fetch)
		{
			MapView.fetch.UpdateMap();
		}
	}
}
