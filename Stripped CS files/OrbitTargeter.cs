using System;
using System.Collections.Generic;
using ns1;
using ns11;
using ns23;
using ns9;
using PreFlightTests;
using UnityEngine;
using Vectrosity;

[RequireComponent(typeof(PatchedConicRenderer))]
public class OrbitTargeter : MonoBehaviour
{
	public enum MenuDrawMode
	{
		const_0,
		PATCH,
		TARGET
	}

	public class Marker
	{
		public MapObject mo;

		public MapNode mn;

		public MapObject.ObjectType otype;

		public MapNode.ApproachNodeType atype;

		public Color color;

		public int pixelSize;

		public bool visible;

		public bool lineOk;

		public VectorLine line;

		public Vector3d pos;

		public OrbitTargeter orbitTargeter;

		public Marker(OrbitTargeter orbitTargeter, string name, Color color, int pixelSize)
		{
			this.orbitTargeter = orbitTargeter;
			this.color = color;
			this.pixelSize = pixelSize;
			otype = MapObject.ObjectType.Generic;
			mn = MapNode.Create(name, color, pixelSize, hoverable: false, pinnable: false, blocksInput: false);
			mn.OnUpdateVisible += OnUpdateVisible;
			mn.OnUpdatePosition += OnUpdatePosition;
			mn.OnUpdateType += OnUpdateType;
			visible = false;
			orbitTargeter.markers.Add(this);
		}

		public Marker(OrbitTargeter orbitTargeter, string name, Orbit patch, MapObject.ObjectType otype, Color color, int pixelSize)
		{
			this.orbitTargeter = orbitTargeter;
			this.color = color;
			this.pixelSize = pixelSize;
			this.otype = otype;
			mo = MapObject.Create(name, name, patch, otype);
			mn = MapNode.Create(mo, color, pixelSize, hoverable: true, pinnable: true, blocksInput: false);
			mn.OnUpdateVisible += OnUpdateVisible;
			mn.OnUpdatePosition += OnUpdatePosition;
			mn.OnUpdateType += OnUpdateType;
			visible = false;
			orbitTargeter.markers.Add(this);
		}

		public void OnUpdateVisible(MapNode mn, MapNode.IconData iData)
		{
			iData.color = color;
			iData.pixelSize = pixelSize;
			iData.visible = visible;
		}

		public Vector3d OnUpdatePosition(MapNode mn)
		{
			return pos;
		}

		public void OnUpdateType(MapNode mn, MapNode.TypeData tData)
		{
			tData.oType = otype;
			tData.aType = atype;
		}

		public void Terminate()
		{
			if (mo != null)
			{
				mo.Terminate();
			}
			if (mn != null)
			{
				mn.Terminate();
			}
			DestroyLine();
			orbitTargeter.markers.Remove(this);
		}

		public void NodeUpdate()
		{
			mn.NodeUpdate();
		}

		public void DestroyLine()
		{
			if (line != null)
			{
				VectorLine.Destroy(ref line);
				line = null;
			}
		}

		public void DrawLine(bool draw3d)
		{
			if (line != null && lineOk)
			{
				if (draw3d)
				{
					line.Draw3D();
				}
				else
				{
					line.Draw();
				}
			}
		}

		public void DottedLineUpdate(ref VectorLine line, Vector3d vector3d_0, Vector3d N2, string name, Color color)
		{
			if (line == null)
			{
				line = new VectorLine(name, new List<Vector3>(256), 5f, LineType.Discrete);
				line.texture = MapView.DottedLinesMaterial.mainTexture;
				line.material = MapView.DottedLinesMaterial;
				line.SetColor(color);
				line.rectTransform.gameObject.layer = 31;
				line.UpdateImmediate = true;
			}
			Vector3[] splinePoints = new Vector3[2] { vector3d_0, N2 };
			line.MakeSpline(splinePoints, 128);
			line.textureScale = 1f;
		}

		public void Update(bool visible, bool lineOk)
		{
			this.visible = visible && orbitTargeter.CanDrawAnyNode();
			this.lineOk = lineOk;
		}
	}

	public class AnDnMarker : Marker
	{
		public bool ascending;

		public Vector3d pos2;

		public double rInc;

		public AnDnMarker(OrbitTargeter orbitTargeter, bool ascending, Orbit patch)
			: base(orbitTargeter, ascending ? "AN" : "DN", patch, ascending ? MapObject.ObjectType.AscendingNode : MapObject.ObjectType.DescendingNode, XKCDColors.ElectricLime, 32)
		{
			this.ascending = ascending;
			mn.OnUpdateCaption += OnUpdateCaption;
		}

		public void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			if (ascending)
			{
				cData.Header = Localizer.Format("#autoLOC_6002405", rInc.ToString("0.0"));
			}
			else
			{
				cData.Header = Localizer.Format("#autoLOC_6002406", (0.0 - rInc).ToString("0.0"));
			}
		}

		public void Update(Vector3d pos, Vector3d pos2, double iInc, bool visible, bool lineOk)
		{
			base.pos = pos;
			this.pos2 = pos2;
			rInc = iInc;
			base.visible = visible && orbitTargeter.CanDrawAnyNode();
			base.lineOk = lineOk;
			DottedLineUpdate(ref line, pos, pos2, "Vector " + (ascending ? "Ascending" : "Descending") + " Node", Color.gray);
		}
	}

	public class ISectMarker : Marker
	{
		public int num;

		public bool target;

		public Vector3d pos2;

		public double separation;

		public double relSpeed;

		public double angle;

		public double double_0;

		public static Color[] isectColors = new Color[2]
		{
			XKCDColors.Orange,
			XKCDColors.Magenta
		};

		public ISectMarker(OrbitTargeter orbitTargeter, int num, bool target, Orbit patch)
			: base(orbitTargeter, "ISect" + num + (target ? "TgT" : ""), patch, MapObject.ObjectType.ApproachIntersect, isectColors[num - 1], 28)
		{
			atype = (target ? MapNode.ApproachNodeType.IntersectOther : MapNode.ApproachNodeType.IntersectOwn);
			this.num = num;
			this.target = target;
			mn.OnUpdateCaption += OnUpdateCaption;
		}

		public void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			if (target)
			{
				cData.Header = Localizer.Format("#autoLOC_197832", num);
				return;
			}
			cData.Header = Localizer.Format("#autoLOC_197834", num);
			cData.captionLine1 = Localizer.Format("#autoLOC_197835", separation.ToString("N1"));
			cData.captionLine2 = ((angle < 0.1) ? Localizer.Format("#autoLOC_7001350", relSpeed.ToString("N1")) : string.Empty);
			cData.captionLine3 = "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - double_0, 3, explicitPositive: true);
		}

		public void Update(Vector3d pos, double separation, double relSpeed, double angle, double double_1, bool visible)
		{
			base.pos = pos;
			this.separation = separation;
			this.relSpeed = relSpeed;
			this.angle = angle;
			double_0 = double_1;
			base.visible = visible && orbitTargeter.CanDrawAnyNode();
		}

		public void Update(Vector3d pos, Vector3d pos2, bool visible, bool lineOk)
		{
			base.pos = pos;
			this.pos2 = pos2;
			base.visible = visible && orbitTargeter.CanDrawAnyNode();
			base.lineOk = lineOk;
			DottedLineUpdate(ref line, pos2, pos, Localizer.Format("#autoLOC_197860", num.ToString()), color);
		}
	}

	public class ClApprMarker : Marker
	{
		public bool target;

		public double separation;

		public double dT;

		public ClApprMarker(OrbitTargeter orbitTargeter, bool target, Orbit patch)
			: base(orbitTargeter, "ClAppr" + (target ? "TgT" : ""), patch, MapObject.ObjectType.ApproachIntersect, XKCDColors.LightCyan, 28)
		{
			atype = (target ? MapNode.ApproachNodeType.CloseApproachOther : MapNode.ApproachNodeType.CloseApproachOwn);
			this.target = target;
			mn.OnUpdateCaption += OnUpdateCaption;
		}

		public void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			if (target)
			{
				cData.Header = cacheAutoLOC_7001351;
				return;
			}
			cData.Header = cacheAutoLOC_7001352;
			cData.captionLine1 = Localizer.Format("#autoLOC_197886", separation.ToString("N1"));
			cData.captionLine2 = "T " + KSPUtil.PrintTime(dT, 3, explicitPositive: true);
		}

		public void Update(Vector3d pos, double separation, double dT, bool visible)
		{
			base.pos = pos;
			this.separation = separation;
			this.dT = dT;
			base.visible = visible && orbitTargeter.CanDrawAnyNode();
		}

		public void Update(Vector3d pos, bool visible)
		{
			base.pos = pos;
			base.visible = visible && orbitTargeter.CanDrawAnyNode();
		}
	}

	public class CursorMarker : Marker
	{
		public CursorMarker(OrbitTargeter orbitTargeter)
			: base(orbitTargeter, "OrbitTargeter Cursor Node", XKCDColors.ElectricLime, 18)
		{
			mn.OnUpdateCaption += OnUpdateCaption;
		}

		public void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			Orbit patch = orbitTargeter.screenCastHit.pr.patch;
			double uTatTA = orbitTargeter.screenCastHit.UTatTA;
			double mouseTA = orbitTargeter.screenCastHit.mouseTA;
			double time = Planetarium.GetUniversalTime() - uTatTA;
			double magnitude = patch.getRelativePositionFromTrueAnomaly(mouseTA).magnitude;
			double orbitalSpeedAtDistance = patch.getOrbitalSpeedAtDistance(magnitude);
			magnitude -= patch.referenceBody.Radius;
			cData.Header = "T " + KSPUtil.PrintTime(time, 3, explicitPositive: true);
			cData.captionLine1 = Localizer.Format("#autoLOC_197927", orbitalSpeedAtDistance.ToString("N0"));
			cData.captionLine2 = Localizer.Format("#autoLOC_197928", magnitude.ToString("N0"));
		}

		public void Update()
		{
			visible = orbitTargeter.orbitHover && orbitTargeter.flightPlanningUnlocked && orbitTargeter.menuDrawMode == MenuDrawMode.const_0 && HighLogic.LoadedSceneIsFlight && !FlightDriver.Pause;
			if (visible)
			{
				color = orbitTargeter.screenCastHit.pr.nodeColor;
			}
			pixelSize = 14;
			pos = orbitTargeter.screenCastHit.orbitPoint;
			NodeUpdate();
		}
	}

	public const float nodeEpsilon = 0.001f;

	public PatchedConicRenderer pcr;

	public PatchedConicRenderer tgtPCR;

	public PatchedConics.PatchCastHit screenCastHit;

	public PatchedConics.PatchCastHit menuCastHit;

	public OrbitRendererBase.OrbitCastHit orbitCastHit;

	public bool AllowPlaceManeuverNode = true;

	public static bool HasManeuverNode;

	public bool orbitHover;

	public bool menuHover;

	public MenuDrawMode menuDrawMode;

	public MapContextMenu ContextMenu;

	public OrbitDriver target;

	public PatchRendering pr;

	public Vessel host;

	public Orbit refPatch;

	public Orbit tgtRefPatch;

	public bool flightPlanningUnlocked;

	public int maxPatchesAheadForAutoWarp = 1;

	public PatchedConicRenderer _pcr;

	public CursorMarker cursorMarker;

	public Vector3 menuScreenPos;

	public Rect menuRect;

	public List<Marker> markers = new List<Marker>();

	public bool draw3dLines;

	public const int dottedLineSegments = 128;

	public bool encountersTarget;

	public const int maxIterations = 20;

	public int iterations;

	public AnDnMarker anMarker;

	public AnDnMarker dnMarker;

	public ISectMarker isect1Marker;

	public ISectMarker isect1TgtMarker;

	public ISectMarker isect2Marker;

	public ISectMarker isect2TgtMarker;

	public ClApprMarker clApprMarker;

	public ClApprMarker clApprTgtMarker;

	public static string cacheAutoLOC_7001351;

	public static string cacheAutoLOC_7001352;

	public bool OrbitHover => orbitHover;

	public void Awake()
	{
		pcr = this.GetComponentCached(ref _pcr);
	}

	public void Start()
	{
		if (ScenarioUpgradeableFacilities.Instance == null)
		{
			flightPlanningUnlocked = true;
		}
		flightPlanningUnlocked = GameVariables.Instance.UnlockedFlightPlanning(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl));
		cursorMarker = new CursorMarker(this);
	}

	public void LateUpdate()
	{
		if (_pcr == null || _pcr.solver == null)
		{
			_pcr = null;
		}
		pcr = this.GetComponentCached(ref _pcr);
		if (pcr.solver == null)
		{
			_pcr = null;
			pcr.solver = GetComponent<PatchedConicSolver>();
		}
		host = pcr.vessel;
		refPatch = ReferencePatchSelect();
		tgtRefPatch = getTargetReferencePatch(refPatch);
		UpdateNodesAndVectors();
		if (!ManeuverGizmoBase.HasMouseFocus && MapView.MapIsEnabled)
		{
			HasManeuverNode = pcr.solver.maneuverNodes.Count > 0;
			if (menuDrawMode == MenuDrawMode.const_0 && !pcr.MouseOverNodes)
			{
				orbitHover = PatchedConics.ScreenCast(Input.mousePosition, (pcr.solver.maneuverNodes.Count > 0) ? pcr.flightPlanRenders : pcr.patchRenders, out screenCastHit, 10f, -1.0, clampToPatches: true);
			}
			else
			{
				orbitHover = false;
			}
			CursorInputUpdate();
		}
		else
		{
			orbitHover = false;
			menuDrawMode = MenuDrawMode.const_0;
		}
	}

	public string GetETAString(double double_0)
	{
		return "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - double_0, 3, explicitPositive: true);
	}

	public void CursorInputUpdate()
	{
		if (ContextMenu != null)
		{
			menuHover = ContextMenu.Hover;
		}
		else
		{
			menuHover = false;
		}
		if (!HighLogic.LoadedSceneIsFlight || host != FlightGlobals.ActiveVessel || FlightDriver.Pause)
		{
			return;
		}
		if (Mouse.Left.GetDoubleClick() && menuDrawMode == MenuDrawMode.TARGET)
		{
			try
			{
				OrbitRendererBase.OrbitCastHit orbitCastHit = (OrbitRendererBase.OrbitCastHit)(object)ContextMenu.sc;
				if (orbitCastHit.driver.Targetable is CelestialBody)
				{
					PlanetariumCamera.fetch.SetTarget(orbitCastHit.driver.celestialBody);
					return;
				}
				if (orbitCastHit.driver.Targetable is Vessel)
				{
					PlanetariumCamera.fetch.SetTarget(((Vessel)orbitCastHit.driver.Targetable).mapObject);
					return;
				}
			}
			catch (Exception)
			{
				Debug.LogWarning("Unable to set focus on double-click");
			}
		}
		if ((!Mouse.Left.GetButtonUp() && (!Mouse.Right.GetButtonUp() || Mouse.Right.WasDragging())) || !InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
		{
			return;
		}
		OrbitDriver orbitDriver = TargetCastNodes(out this.orbitCastHit);
		if (orbitDriver != null && Mouse.Left.GetButtonUp())
		{
			SetMenuMode(MenuDrawMode.TARGET, orbitDriver);
		}
		else if (!ManeuverGizmoBase.HasMouseFocus)
		{
			if (orbitHover && flightPlanningUnlocked && Mouse.Left.GetButtonUp())
			{
				SetMenuMode(MenuDrawMode.PATCH);
			}
			else
			{
				if (menuHover)
				{
					return;
				}
				if (menuDrawMode != 0)
				{
					SetMenuMode(MenuDrawMode.const_0);
					return;
				}
				orbitDriver = TargetCastSplines(out this.orbitCastHit);
				if (orbitDriver != null && Mouse.Left.GetButtonUp())
				{
					SetMenuMode(MenuDrawMode.TARGET, orbitDriver);
				}
				else
				{
					SetMenuMode(MenuDrawMode.const_0);
				}
			}
		}
		else
		{
			SetMenuMode(MenuDrawMode.const_0);
		}
	}

	public void SetMenuMode(MenuDrawMode m, OrbitDriver tgt = null)
	{
		if (ContextMenu != null)
		{
			ContextMenu.Dismiss();
			menuDrawMode = MenuDrawMode.const_0;
			return;
		}
		switch (m)
		{
		case MenuDrawMode.PATCH:
			menuCastHit = screenCastHit;
			ContextMenu = MapContextMenu.Create(GetETAString(menuCastHit.UTatTA), menuCastHit, onContextMenuDismiss, new AddManeuver(pcr, menuCastHit.UTatTA), new AutoWarpToUT(menuCastHit.UTatTA, pcr, menuCastHit, maxPatchesAheadForAutoWarp));
			break;
		case MenuDrawMode.TARGET:
			menuCastHit = screenCastHit;
			ContextMenu = MapContextMenu.Create(Localizer.Format("#autoLOC_7001301", orbitCastHit.or.discoveryInfo.displayName.Value), orbitCastHit, onContextMenuDismiss, new SetAsTarget(tgt.Targetable, () => FlightGlobals.fetch.VesselTarget), new FocusObject(tgt));
			break;
		}
		menuDrawMode = m;
	}

	public void onContextMenuDismiss()
	{
		menuDrawMode = MenuDrawMode.const_0;
		ContextMenu = null;
	}

	public void onViewInTrackingStationButton()
	{
		menuDrawMode = MenuDrawMode.const_0;
		InputLockManager.SetControlLock("OrbitTargeterCheckingTrackingStationState");
		PreFlightCheck preFlightCheck = new PreFlightCheck(onTrackingStationProceed, onTrackingStationDismiss);
		preFlightCheck.AddTest(new FacilityOperational("TrackingStation", "Tracking Station"));
		preFlightCheck.RunTests();
	}

	public void onTrackingStationProceed()
	{
		MapView.ExitMapView();
		SpaceTracking.GoToAndFocusVessel(orbitCastHit.driver.vessel);
		menuDrawMode = MenuDrawMode.const_0;
		onTrackingStationDismiss();
	}

	public void onTrackingStationDismiss()
	{
		InputLockManager.RemoveControlLock("OrbitTargeterCheckingTrackingStationState");
	}

	public bool IsTargetCb(CelestialBody refBody, CelestialBody tgtCb)
	{
		if ((bool)tgtCb)
		{
			return refBody == tgtCb;
		}
		return false;
	}

	public PatchRendering FindPatch(List<PatchRendering> patchList, int startIndex, int endIndex, CelestialBody refCb, CelestialBody tgtCb = null)
	{
		PatchRendering result = patchList[startIndex];
		int num = startIndex;
		while (num >= endIndex)
		{
			PatchRendering patchRendering = patchList[num];
			if (!(patchRendering.patch.referenceBody == refCb) && !IsTargetCb(patchRendering.patch.referenceBody, tgtCb))
			{
				num--;
				continue;
			}
			result = patchList[num];
			if (IsTargetCb(patchRendering.patch.referenceBody, tgtCb) && num > endIndex)
			{
				result = patchList[num - 1];
				encountersTarget = true;
			}
			break;
		}
		return result;
	}

	public Orbit ReferencePatchSelect()
	{
		encountersTarget = false;
		List<PatchRendering> list;
		int num;
		int value;
		if (pcr.solver.maneuverNodes.Count > 0 && pcr.flightPlanRenders.Count > 0)
		{
			list = pcr.flightPlanRenders;
			num = pcr.flightPlanRenders.Count - 1;
			int index = pcr.solver.maneuverNodes.Count - 1;
			ManeuverNode maneuverNode = pcr.solver.maneuverNodes[index];
			Orbit item = pcr.solver.FindPatchContainingUT(maneuverNode.double_0);
			value = pcr.solver.patches.IndexOf(item);
			value = Mathf.Clamp(value, 0, list.Count - 1);
		}
		else
		{
			list = pcr.patchRenders;
			num = pcr.solver.patchesAhead;
			value = 0;
		}
		if (target != null && (bool)target.celestialBody)
		{
			if (target.celestialBody == list[value].patch.referenceBody)
			{
				for (int i = value; i < num; i++)
				{
					if (target.celestialBody != list[i].patch.referenceBody)
					{
						value = i;
						break;
					}
				}
			}
			if (value > 0 && target.celestialBody == list[value - 1].patch.referenceBody)
			{
				int i = value - 1;
				while (i >= 0)
				{
					if (!(target.celestialBody != list[i].patch.referenceBody))
					{
						i--;
						continue;
					}
					num = i;
					break;
				}
			}
			while (num > value && target.celestialBody == list[num].patch.referenceBody)
			{
				num--;
			}
			pr = FindPatch(list, num, value, target.orbit.referenceBody, target.celestialBody);
		}
		else if (target != null && (bool)target.vessel)
		{
			pr = FindPatch(list, num, value, target.orbit.referenceBody);
		}
		else
		{
			pr = list[num];
		}
		return pr.patch;
	}

	public Orbit getTargetReferencePatch(Orbit refPatch)
	{
		if (target != null)
		{
			if ((bool)target.vessel && target.vessel.PatchedConicsAttached)
			{
				for (int i = 0; i <= target.vessel.patchedConicSolver.patchesAhead; i++)
				{
					Orbit orbit = target.vessel.patchedConicSolver.patches[i];
					if (orbit.referenceBody == refPatch.referenceBody)
					{
						return orbit;
					}
				}
			}
			return target.orbit;
		}
		return null;
	}

	public OrbitDriver TargetCastNodes(out OrbitRendererBase.OrbitCastHit orbitHit)
	{
		orbitHit = default(OrbitRendererBase.OrbitCastHit);
		int count = Planetarium.Orbits.Count;
		for (int i = 0; i < count; i++)
		{
			OrbitDriver orbitDriver = Planetarium.Orbits[i];
			if (!(orbitDriver == pcr.solver.obtDriver) && !(orbitDriver.celestialBody == refPatch.referenceBody) && !(orbitDriver.Renderer == null) && MapViewFiltering.CheckAgainstFilter(orbitDriver.vessel) && orbitDriver.Renderer.mouseOver)
			{
				orbitHit.driver = orbitDriver;
				orbitHit.or = orbitDriver.Renderer;
				orbitHit.mouseTA = orbitDriver.orbit.trueAnomaly;
				break;
			}
		}
		return orbitHit.driver;
	}

	public OrbitDriver TargetCastSplines(out OrbitRendererBase.OrbitCastHit orbitHit)
	{
		orbitHit = default(OrbitRendererBase.OrbitCastHit);
		OrbitRendererBase.OrbitCastHit hitInfo = default(OrbitRendererBase.OrbitCastHit);
		int count = Planetarium.Orbits.Count;
		for (int i = 0; i < count; i++)
		{
			OrbitDriver orbitDriver = Planetarium.Orbits[i];
			if (!(orbitDriver == pcr.solver.obtDriver) && !(orbitDriver.Renderer == null) && MapViewFiltering.CheckAgainstFilter(orbitDriver.vessel) && orbitDriver.Renderer.OrbitCast(Input.mousePosition, out hitInfo, 18f))
			{
				orbitHit = hitInfo;
				break;
			}
		}
		return orbitHit.driver;
	}

	public void SetTarget(OrbitDriver tgt)
	{
		if (target != null)
		{
			target.Renderer.isFocused = false;
			if ((bool)pcr && (bool)pcr.solver)
			{
				pcr.solver.targetBody = null;
			}
			if ((bool)target.vessel && target.vessel.PatchedConicsAttached)
			{
				target.vessel.DetachPatchedConicsSolver();
			}
		}
		target = tgt;
		tgtPCR = null;
		if (target != null)
		{
			MapObject data = ((target.vessel == null) ? target.celestialBody.MapObject : target.vessel.mapObject);
			GameEvents.OnTargetObjectChanged.Fire(data);
			target.Renderer.isFocused = true;
			if ((bool)target.celestialBody)
			{
				if ((bool)pcr && (bool)pcr.solver)
				{
					pcr.solver.targetBody = target.celestialBody;
				}
			}
			else if ((bool)target.vessel)
			{
				target.vessel.AttachPatchedConicsSolver();
			}
		}
		else
		{
			GameEvents.OnTargetObjectChanged.Fire(null);
		}
		if ((bool)pcr && (bool)pcr.solver)
		{
			Vector3d deltaV = Vector3d.zero;
			bool flag = false;
			if (pcr.solver.maneuverNodes.Count > 0 && Math.Abs(pcr.solver.maneuverNodes[0].GetBurnVector(pcr.solver.maneuverNodes[0].patch).magnitude - pcr.solver.maneuverNodes[0].DeltaV.magnitude) > 0.0010000000474974513)
			{
				deltaV = pcr.solver.maneuverNodes[0].DeltaV;
				pcr.solver.maneuverNodes[0].DeltaV = pcr.solver.maneuverNodes[0].GetPartialDv();
				flag = true;
			}
			pcr.solver.UpdateFlightPlan();
			if (flag)
			{
				pcr.solver.maneuverNodes[0].DeltaV = deltaV;
			}
		}
	}

	public void UpdateISectMarkers(ref ISectMarker isectMarker, ref ISectMarker isectTgtMarker, int num, double EVpUT, double EVp, double EVs)
	{
		PatchRendering patchRendering = null;
		if (PatchedConics.TAIsWithinPatchBounds(EVp, refPatch))
		{
			Orbit item = tgtPCR.solver.FindPatchContainingUT(EVpUT);
			int num2 = tgtPCR.solver.patches.IndexOf(item);
			if (num2 >= 0)
			{
				patchRendering = tgtPCR.patchRenders[num2];
			}
		}
		if (patchRendering != null)
		{
			if (isectMarker == null)
			{
				isectMarker = new ISectMarker(this, num, target: false, refPatch);
			}
			if (isectTgtMarker == null)
			{
				isectTgtMarker = new ISectMarker(this, num, target: true, tgtRefPatch);
			}
			Vector3d relativePositionAtUT = refPatch.getRelativePositionAtUT(EVpUT);
			Vector3d relativePositionAtUT2 = tgtRefPatch.getRelativePositionAtUT(EVpUT);
			Vector3d orbitalVelocityAtUT = refPatch.getOrbitalVelocityAtUT(EVpUT);
			Vector3d orbitalVelocityAtUT2 = tgtRefPatch.getOrbitalVelocityAtUT(EVpUT);
			Vector3d vector3d = pr.GetScaledSpacePointFromTA(EVp, EVpUT);
			Vector3d vector3d2 = ScaledSpace.LocalToScaledSpace(tgtRefPatch.getPositionAtUT(EVpUT));
			double separation = (relativePositionAtUT - relativePositionAtUT2).magnitude / 1000.0;
			double magnitude = (orbitalVelocityAtUT - orbitalVelocityAtUT2).magnitude;
			double angle = Math.Acos(Vector3d.Dot((vector3d - pr.cb).normalized, (vector3d2 - pr.cb).normalized));
			isectMarker.Update(vector3d, separation, magnitude, angle, EVpUT, visible: true);
			isectTgtMarker.Update(vector3d2, patchRendering.cb, visible: true, lineOk: true);
		}
		else
		{
			if (isectMarker != null)
			{
				isectMarker.visible = false;
			}
			if (isectTgtMarker != null)
			{
				isectTgtMarker.visible = false;
				isectTgtMarker.lineOk = false;
			}
		}
	}

	public void UpdateNodesAndVectors()
	{
		for (int num = markers.Count - 1; num >= 0; num--)
		{
			if (markers[num] != cursorMarker)
			{
				markers[num].Update(visible: false, lineOk: false);
			}
		}
		if (MapView.Draw3DLines != draw3dLines)
		{
			draw3dLines = MapView.Draw3DLines;
			for (int num2 = markers.Count - 1; num2 >= 0; num2--)
			{
				markers[num2].DestroyLine();
			}
		}
		if (CanDrawAnyLines() && target != null)
		{
			if (DropInvalidTargets())
			{
				return;
			}
			if (refPatch.referenceBody == tgtRefPatch.referenceBody)
			{
				Vector3d orbitNormal = refPatch.GetOrbitNormal();
				Vector3d orbitNormal2 = tgtRefPatch.GetOrbitNormal();
				Vector3d normalized = Vector3d.Cross(orbitNormal2, orbitNormal).normalized;
				double iInc = Math.Acos(Vector3.Dot(orbitNormal2.normalized, orbitNormal.normalized)) * 57.295780181884766;
				double trueAnomalyOfZupVector = refPatch.GetTrueAnomalyOfZupVector(normalized);
				double trueAnomalyOfZupVector2 = tgtRefPatch.GetTrueAnomalyOfZupVector(normalized);
				if (PatchedConics.TAIsWithinPatchBounds(trueAnomalyOfZupVector, refPatch) && PatchedConics.TAIsWithinPatchBounds(trueAnomalyOfZupVector2, tgtRefPatch))
				{
					Vector3d pos = pr.GetScaledSpacePointFromTA(trueAnomalyOfZupVector, refPatch.StartUT + refPatch.GetDTforTrueAnomaly(trueAnomalyOfZupVector, 0.0));
					Vector3d pos2 = ScaledSpace.LocalToScaledSpace(tgtRefPatch.referenceBody.position + normalized.xzy.normalized * tgtRefPatch.RadiusAtTrueAnomaly(trueAnomalyOfZupVector2));
					if (anMarker == null)
					{
						anMarker = new AnDnMarker(this, ascending: true, pr.patch);
					}
					anMarker.Update(pos, pos2, iInc, visible: true, lineOk: true);
				}
				else if (anMarker != null)
				{
					anMarker.visible = false;
					anMarker.lineOk = false;
				}
				double num3 = (trueAnomalyOfZupVector + Math.PI) % (Math.PI * 2.0);
				double tA = (trueAnomalyOfZupVector2 + Math.PI) % (Math.PI * 2.0);
				if (PatchedConics.TAIsWithinPatchBounds(num3, refPatch) && PatchedConics.TAIsWithinPatchBounds(tA, tgtRefPatch))
				{
					Vector3d pos3 = pr.GetScaledSpacePointFromTA(num3, refPatch.StartUT + refPatch.GetDTforTrueAnomaly(num3, 0.0));
					Vector3d pos4 = ScaledSpace.LocalToScaledSpace(tgtRefPatch.referenceBody.position - normalized.xzy.normalized * tgtRefPatch.RadiusAtTrueAnomaly(tA));
					if (dnMarker == null)
					{
						dnMarker = new AnDnMarker(this, ascending: false, pr.patch);
					}
					dnMarker.Update(pos3, pos4, iInc, visible: true, lineOk: true);
				}
				else if (dnMarker != null)
				{
					dnMarker.visible = false;
					dnMarker.lineOk = false;
				}
			}
			if ((bool)target.vessel && tgtRefPatch.referenceBody == refPatch.referenceBody && Orbit.PeApIntersects(refPatch, tgtRefPatch, 10000.0))
			{
				double double_ = 0.0;
				double double_2 = 0.0;
				double FFp = 0.0;
				double FFs = 0.0;
				double SFp = 0.0;
				double SFs = 0.0;
				int num4 = Orbit.FindClosestPoints(refPatch, tgtRefPatch, ref double_, ref double_2, ref FFp, ref FFs, ref SFp, ref SFs, 0.0001, 20, ref iterations);
				double a = refPatch.StartUT + refPatch.GetDTforTrueAnomaly(FFp, 0.0);
				double b = refPatch.StartUT + refPatch.GetDTforTrueAnomaly(SFp, 0.0);
				if (a > b)
				{
					UtilMath.SwapValues(ref a, ref b);
					UtilMath.SwapValues(ref FFp, ref SFp);
					UtilMath.SwapValues(ref FFs, ref SFs);
				}
				if (!tgtPCR)
				{
					tgtPCR = target.GetComponent<PatchedConicRenderer>();
				}
				UpdateISectMarkers(ref isect1Marker, ref isect1TgtMarker, 1, a, FFp, FFs);
				if (num4 > 1)
				{
					UpdateISectMarkers(ref isect2Marker, ref isect2TgtMarker, 2, b, SFp, SFs);
				}
			}
			if ((bool)target.celestialBody && !encountersTarget && refPatch.closestTgtApprUT != 0.0)
			{
				if (clApprMarker == null)
				{
					clApprMarker = new ClApprMarker(this, target: false, pr.patch);
				}
				if (clApprTgtMarker == null)
				{
					clApprTgtMarker = new ClApprMarker(this, target: true, pr.patch);
				}
				Vector3d relativePositionAtUT = refPatch.getRelativePositionAtUT(refPatch.closestTgtApprUT);
				Vector3d relativePositionAtUT2 = tgtRefPatch.getRelativePositionAtUT(refPatch.closestTgtApprUT);
				double separation = (relativePositionAtUT - relativePositionAtUT2).magnitude * 0.001;
				Vector3d pos5 = pr.GetScaledSpacePointFromTA(refPatch.TrueAnomalyAtUT(refPatch.closestTgtApprUT), refPatch.closestTgtApprUT);
				Vector3d pos6 = ScaledSpace.LocalToScaledSpace(tgtRefPatch.getPositionAtUT(refPatch.closestTgtApprUT));
				double dT = Planetarium.GetUniversalTime() - refPatch.closestTgtApprUT;
				clApprMarker.Update(pos5, separation, dT, visible: true);
				clApprTgtMarker.Update(pos6, visible: true);
			}
			else
			{
				if (clApprMarker != null)
				{
					clApprMarker.visible = false;
				}
				if (clApprTgtMarker != null)
				{
					clApprTgtMarker.visible = false;
				}
			}
		}
		for (int num5 = markers.Count - 1; num5 >= 0; num5--)
		{
			markers[num5].DrawLine(MapView.Draw3DLines);
			if (markers[num5] != cursorMarker)
			{
				markers[num5].NodeUpdate();
			}
		}
		if (cursorMarker != null)
		{
			cursorMarker.Update();
		}
		NodeCleanup(willDestroy: false);
	}

	public Marker CleanupMarker(Marker marker, bool force)
	{
		if (marker != null && (!marker.visible || force))
		{
			marker.Terminate();
			marker = null;
		}
		return marker;
	}

	public void NodeCleanup(bool willDestroy)
	{
		bool force = !MapView.MapIsEnabled || willDestroy;
		anMarker = (AnDnMarker)CleanupMarker(anMarker, force);
		dnMarker = (AnDnMarker)CleanupMarker(dnMarker, force);
		isect1Marker = (ISectMarker)CleanupMarker(isect1Marker, force);
		isect1TgtMarker = (ISectMarker)CleanupMarker(isect1TgtMarker, force);
		isect2Marker = (ISectMarker)CleanupMarker(isect2Marker, force);
		isect2TgtMarker = (ISectMarker)CleanupMarker(isect2TgtMarker, force);
		clApprMarker = (ClApprMarker)CleanupMarker(clApprMarker, force);
		clApprTgtMarker = (ClApprMarker)CleanupMarker(clApprTgtMarker, force);
		if (willDestroy && cursorMarker != null)
		{
			cursorMarker.Terminate();
			cursorMarker = null;
		}
	}

	public bool DropInvalidTargets()
	{
		if (target.celestialBody == refPatch.referenceBody)
		{
			Debug.Log("[Orbit Targeter]: Dropping target because it is the current main body", target.gameObject);
			FlightGlobals.fetch.SetVesselTarget(null, overrideInputLock: true);
			return true;
		}
		if (target.celestialBody != null && refPatch.referenceBody.HasParent(target.celestialBody))
		{
			Debug.Log("[Orbit Targeter]: Dropping target because it is a parent of the current main body", target.gameObject);
			FlightGlobals.fetch.SetVesselTarget(null, overrideInputLock: true);
			return true;
		}
		return false;
	}

	public bool CanDrawAnyLines()
	{
		if (MapView.MapIsEnabled)
		{
			return !pcr.vessel.LandedOrSplashed;
		}
		return false;
	}

	public bool CanDrawAnyNode()
	{
		if (MapView.MapIsEnabled)
		{
			return target != null;
		}
		return false;
	}

	public Vector2d GetSeparations()
	{
		double x = ((isect1Marker != null) ? isect1Marker.separation : 0.0);
		double y = ((isect2Marker != null) ? isect2Marker.separation : 0.0);
		return new Vector2d(x, y);
	}

	public void ClearMenus()
	{
		orbitHover = false;
		menuDrawMode = MenuDrawMode.const_0;
	}

	public void OnDestroy()
	{
		NodeCleanup(willDestroy: true);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7001351 = Localizer.Format("#autoLOC_7001351");
		cacheAutoLOC_7001352 = Localizer.Format("#autoLOC_7001352");
	}
}
