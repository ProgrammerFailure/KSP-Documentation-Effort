using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitRenderer : OrbitRendererBase
{
	public bool orbitDisplayUnlocked;

	public override void Start()
	{
		base.Start();
		if ((bool)vessel)
		{
			base.objName = vessel.GetDisplayName();
			discoveryInfo = vessel.DiscoveryInfo;
			orbitDisplayUnlocked = GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) >= GameVariables.OrbitDisplayMode.AllOrbits;
			SetColor(new Color(0.71f, 0.71f, 0.71f, 1f));
		}
		else if ((bool)celestialBody)
		{
			base.objName = base.name;
			discoveryInfo = celestialBody.DiscoveryInfo;
			orbitDisplayUnlocked = GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) >= GameVariables.OrbitDisplayMode.CelestialBodyOrbits;
			if (PSystemManager.OrbitRendererDataCache.ContainsKey(celestialBody))
			{
				OrbitRendererData orbitRendererData = PSystemManager.OrbitRendererDataCache[celestialBody];
				orbitColor = orbitRendererData.orbitColor;
				nodeColor = orbitRendererData.nodeColor;
				lowerCamVsSmaRatio = orbitRendererData.lowerCamVsSmaRatio;
				upperCamVsSmaRatio = orbitRendererData.upperCamVsSmaRatio;
			}
		}
		else if ((bool)meNode)
		{
			base.objName = base.name;
			discoveryInfo = new DiscoveryInfo(meNode);
			orbitDisplayUnlocked = true;
		}
		else
		{
			base.objName = base.name;
			orbitDisplayUnlocked = true;
		}
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
	}

	public void OnSceneChange(GameScenes scene)
	{
		Object.Destroy(this);
	}

	public override void DrawOrbit(DrawMode mode)
	{
		if (!(MapView.fetch == null) && MapView.MapIsEnabled && !(PlanetariumCamera.fetch.target == null) && base.OrbitLine != null)
		{
			bool flag = IsDebris();
			bool flag2 = MapViewFiltering.CheckAgainstFilter(vessel);
			base.OrbitLine.active = GetActive(mode, flag, flag2);
			if ((!flag || base.mouseOver) && flag2)
			{
				base.DrawOrbit(mode);
			}
		}
	}

	public bool GetActive(DrawMode mode, bool isDebris, bool isUnfiltered)
	{
		return mode != 0 && lineOpacity > 0f && (!isDebris || base.mouseOver) && orbitDisplayUnlocked && discoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors) && isUnfiltered;
	}

	public bool IsDebris()
	{
		if ((bool)vessel && vessel.vesselType == VesselType.Debris)
		{
			return !isFocused;
		}
		return false;
	}

	public override Color GetNodeColour()
	{
		if (!isFocused)
		{
			return nodeColor.smethod_0(lineOpacity);
		}
		return XKCDColors.ElectricLime;
	}

	public override Color GetOrbitColour()
	{
		if (!isFocused)
		{
			return orbitColor.smethod_0(lineOpacity);
		}
		return XKCDColors.ElectricLime;
	}

	public override bool CanDrawAnyIcons()
	{
		if (!MapView.MapIsEnabled)
		{
			return false;
		}
		return base.CanDrawAnyIcons();
	}

	public override bool OrbitCast(Vector3 screenPos, out OrbitCastHit hitInfo, float orbitPixelWidth = 10f)
	{
		hitInfo = default(OrbitCastHit);
		hitInfo.or = this;
		hitInfo.driver = driver;
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return false;
		}
		return base.OrbitCast(screenPos, out hitInfo, orbitPixelWidth);
	}
}
