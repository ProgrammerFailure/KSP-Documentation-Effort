using System;
using FinePrint;
using FinePrint.Utilities;
using ns23;
using UnityEngine;

public class OrbitTargetRenderer : OrbitRenderer
{
	public OrbitSnapshot snapshot;

	public float animationTimerMax = 8f;

	public float animationTimerCurrent;

	public bool activeDraw;

	public const string endColor = "</color>";

	public string startColor = "<color=#ffffff>";

	public CelestialBody focusBody;

	public bool activeFlight;

	public bool ANDNVisible;

	public double relativeInclination;

	public Vessel targetVessel;

	public static T Setup<T>(string name, int seed, Orbit orbit, bool activedraw = true) where T : OrbitTargetRenderer
	{
		Color color = SystemUtilities.RandomColor(seed, 1f, 1f, 1f);
		GameObject obj = new GameObject(name);
		T val = obj.AddComponent(typeof(T)) as T;
		OrbitDriver orbitDriver = obj.AddComponent<OrbitDriver>();
		orbitDriver.orbit = orbit;
		orbitDriver.orbitColor = color;
		val.snapshot = new OrbitSnapshot(orbit);
		val.drawNodes = true;
		val.drawIcons = DrawIcons.const_3;
		val.drawMode = DrawMode.REDRAW_AND_RECALCULATE;
		val.animationTimerMax = ContractDefs.Satellite.AnimationDuration;
		val.SetColor(color);
		val.autoTextureOffset = false;
		val.driver = orbitDriver;
		orbitDriver.Renderer = val;
		orbitDriver.enabled = false;
		val.activeDraw = activedraw;
		if (ScaledSpace.Instance != null)
		{
			val.transform.parent = ScaledSpace.Instance.transform;
		}
		return val;
	}

	public void Cleanup()
	{
		drawNodes = false;
		drawIcons = DrawIcons.NONE;
		drawMode = DrawMode.const_0;
		if (base.gameObject != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Update()
	{
		if (MapView.MapIsEnabled)
		{
			UpdateLocals();
			CheckVisibility();
			if (drawMode != 0)
			{
				LockOrbit();
				AnimateOrbit();
			}
		}
	}

	public virtual void UpdateLocals()
	{
		focusBody = CelestialUtilities.MapFocusBody();
		activeFlight = HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && targetVessel != null;
		ANDNVisible = false;
		if (activeFlight && targetVessel.orbit != null && targetVessel.mainBody != null && targetVessel.mainBody == base.orbit.referenceBody && targetVessel.orbit.eccentricity < 1.0 && targetVessel.orbit.ApR < targetVessel.mainBody.sphereOfInfluence)
		{
			ANDNVisible = true;
		}
		relativeInclination = 0.0;
		if (activeFlight && !(targetVessel.mainBody != focusBody))
		{
			relativeInclination = Math.Round(OrbitUtilities.GetRelativeInclination(targetVessel.orbit, base.orbit), 1);
			if (double.IsNaN(relativeInclination))
			{
				relativeInclination = 0.0;
			}
		}
	}

	public void CheckVisibility()
	{
		drawMode = ((focusBody != null && focusBody == base.orbit.referenceBody && activeDraw) ? DrawMode.REDRAW_AND_RECALCULATE : DrawMode.const_0);
	}

	public void LockOrbit()
	{
		driver.orbit.SetOrbit(snapshot.inclination, snapshot.eccentricity, snapshot.semiMajorAxis, snapshot.double_0, snapshot.argOfPeriapsis, snapshot.meanAnomalyAtEpoch, snapshot.epoch, FlightGlobals.Bodies[snapshot.ReferenceBodyIndex]);
	}

	public void AnimateOrbit()
	{
		animationTimerCurrent = (animationTimerCurrent + Time.deltaTime) % animationTimerMax;
		double num = base.orbit.EccentricAnomalyAtUT(base.orbit.period * (double)(animationTimerCurrent / animationTimerMax));
		textureOffset = 1f - (float)((num + UtilMath.TwoPI) % UtilMath.TwoPI / UtilMath.TwoPI);
	}

	public override bool CanDrawAnyIcons()
	{
		if (!MapView.MapIsEnabled)
		{
			return false;
		}
		if (!activeDraw)
		{
			return false;
		}
		if (drawNodes && drawIcons != 0)
		{
			if (!MapViewFiltering.CheckAgainstFilter(vessel))
			{
				return false;
			}
			if (discoveryInfo.Level == DiscoveryLevels.None)
			{
				return false;
			}
			if (lineOpacity <= 0f)
			{
				return false;
			}
			if (!(focusBody == null) && !(focusBody != base.orbit.referenceBody))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public override void ANDNNodes_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		data.color = GetNodeColour();
		data.visible = ANDNVisible && CanDrawAnyIcons() && HaveStateVectorKnowledge() && GetCurrentDrawMode() == DrawIcons.const_3 && activeDraw;
	}

	public override Vector3d ascNode_OnUpdatePosition(MapNode n)
	{
		if (ANDNVisible && activeDraw)
		{
			double num = OrbitUtilities.AngleOfDescendingNode(base.orbit, FlightGlobals.ActiveVessel.orbit);
			return ScaledSpace.LocalToScaledSpace(base.orbit.getPositionFromTrueAnomaly(num * (Math.PI / 180.0)));
		}
		return Vector3d.zero;
	}

	public override Vector3d descNode_OnUpdatePosition(MapNode n)
	{
		if (ANDNVisible && activeDraw)
		{
			double num = OrbitUtilities.AngleOfAscendingNode(base.orbit, FlightGlobals.ActiveVessel.orbit);
			return ScaledSpace.LocalToScaledSpace(base.orbit.getPositionFromTrueAnomaly(num * (Math.PI / 180.0)));
		}
		return Vector3d.zero;
	}
}
