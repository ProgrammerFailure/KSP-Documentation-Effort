using System;
using System.Collections.Generic;
using ns23;
using ns9;
using UnityEngine;

[RequireComponent(typeof(PatchedConicSolver))]
public class PatchedConicRenderer : MonoBehaviour
{
	public int patchSamples = 48;

	public int interpolations = 4;

	public PatchedConicSolver solver;

	public List<PatchRendering> patchRenders;

	public List<PatchRendering> flightPlanRenders;

	public PatchRendering.RelativityMode relativityMode = PatchRendering.RelativityMode.RELATIVE;

	public CelestialBody relativeTo;

	public bool drawTimes;

	public bool renderEnabled = true;

	public bool mouseOverGizmos;

	public bool mouseOverNodes;

	public Vessel vessel => obtDriver.vessel;

	public OrbitDriver obtDriver => solver.obtDriver;

	public Orbit orbit => obtDriver.orbit;

	public bool MouseOverNodes
	{
		get
		{
			if (!mouseOverGizmos)
			{
				return mouseOverNodes;
			}
			return true;
		}
	}

	public Color GetPatchColor(int index, Vessel vessel)
	{
		if (!(vessel == FlightGlobals.ActiveVessel))
		{
			return MapView.TargetPatchColors[index % MapView.TargetPatchColors.Length];
		}
		return MapView.PatchColors[index % MapView.PatchColors.Length];
	}

	public void Awake()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
	}

	public void Start()
	{
		solver = GetComponent<PatchedConicSolver>();
		patchRenders = new List<PatchRendering>();
		flightPlanRenders = new List<PatchRendering>();
		relativeTo = FlightGlobals.Bodies[1];
		relativityMode = (PatchRendering.RelativityMode)GameSettings.CONIC_PATCH_DRAW_MODE;
		for (int i = 0; i < solver.maxTotalPatches; i++)
		{
			PatchRendering patchRendering = new PatchRendering(vessel.name + " patch " + patchRenders.Count, patchSamples, interpolations, solver.patches[i], MapView.OrbitLinesMaterial, 5f, smoothTexture: true, this);
			patchRenders.Add(patchRendering);
			patchRendering.SetColor(GetPatchColor(i, vessel));
			patchRendering.relativityMode = relativityMode;
			patchRendering.relativeTo = relativeTo;
		}
	}

	public void OnGameSettingsApplied()
	{
		if (relativityMode != (PatchRendering.RelativityMode)GameSettings.CONIC_PATCH_DRAW_MODE)
		{
			relativityMode = (PatchRendering.RelativityMode)GameSettings.CONIC_PATCH_DRAW_MODE;
		}
	}

	public void LateUpdate()
	{
		if (Planetarium.Pause || Time.timeScale == 0f)
		{
			return;
		}
		if (!MapView.MapIsEnabled)
		{
			int i = 0;
			for (int count = patchRenders.Count; i < count; i++)
			{
				patchRenders[i].UpdateUINodes();
				patchRenders[i].DestroyVector();
			}
			int count2 = solver.maneuverNodes.Count;
			while (count2-- > 0)
			{
				ManeuverNode maneuverNode = solver.maneuverNodes[count2];
				maneuverNode.DetachGizmo();
				if (maneuverNode.scaledSpaceTarget != null && maneuverNode.scaledSpaceTarget.uiNode != null)
				{
					maneuverNode.scaledSpaceTarget.uiNode.Terminate();
				}
			}
			for (int j = 0; j < flightPlanRenders.Count; j++)
			{
				flightPlanRenders[j].UpdateUINodes();
				flightPlanRenders[j].DestroyVector();
			}
		}
		else if (obtDriver.updateMode != OrbitDriver.UpdateMode.IDLE && (!obtDriver.vessel || !obtDriver.vessel.LandedOrSplashed))
		{
			renderEnabled = true;
			mouseOverNodes = false;
			int num = 0;
			int k = 0;
			for (int count3 = patchRenders.Count; k < count3; k++)
			{
				if (solver.patches[k].activePatch && MapView.MapIsEnabled)
				{
					patchRenders[k].patch = solver.patches[k];
					patchRenders[k].currentMainBody = orbit.referenceBody;
					patchRenders[k].relativityMode = relativityMode;
					patchRenders[k].SetColor(GetPatchColor(k, vessel));
					patchRenders[k].UpdatePR();
				}
				else
				{
					patchRenders[k].UpdateUINodes();
					patchRenders[k].DestroyVector();
				}
				if (k < solver.flightPlan.Count && solver.flightPlan[k] == solver.patches[k])
				{
					num = k;
				}
			}
			for (int l = 0; l < solver.flightPlan.Count; l++)
			{
				if (flightPlanRenders.Count <= l)
				{
					PatchRendering item = new PatchRendering(vessel.name + " flight plan " + flightPlanRenders.Count, patchSamples, interpolations, solver.flightPlan[l], MapView.DottedLinesMaterial, 5f, smoothTexture: false, this);
					flightPlanRenders.Add(item);
				}
				if (MapView.MapIsEnabled)
				{
					flightPlanRenders[l].patch = solver.flightPlan[l];
					flightPlanRenders[l].currentMainBody = orbit.referenceBody;
					flightPlanRenders[l].relativityMode = relativityMode;
					flightPlanRenders[l].SetColor(GetPatchColor(Math.Max(l - num, 0), vessel));
					flightPlanRenders[l].visible = l > num;
					flightPlanRenders[l].UpdatePR();
				}
				else
				{
					flightPlanRenders[l].UpdateUINodes();
					flightPlanRenders[l].DestroyVector();
				}
			}
			while (flightPlanRenders.Count > solver.flightPlan.Count)
			{
				flightPlanRenders[flightPlanRenders.Count - 1].DestroyVector();
				flightPlanRenders[flightPlanRenders.Count - 1].DestroyUINodes();
				flightPlanRenders.RemoveAt(flightPlanRenders.Count - 1);
			}
			int count4 = solver.maneuverNodes.Count;
			while (count4-- > 0)
			{
				ManeuverNode maneuverNode2 = solver.maneuverNodes[count4];
				if (MapView.MapIsEnabled)
				{
					if (maneuverNode2.nextPatch == null)
					{
						continue;
					}
					PatchRendering patchRendering = FindRenderingForPatch(maneuverNode2.nextPatch);
					if (patchRendering == null)
					{
						if (maneuverNode2.patch.previousPatch != null)
						{
							patchRendering = FindRenderingForPatch(maneuverNode2.patch.previousPatch);
						}
						if (patchRendering == null)
						{
							Debug.LogError("[Maneuver Solver Error]: No patch found for node at UT:" + maneuverNode2.double_0.ToString("0.0"), maneuverNode2.scaledSpaceTarget);
							continue;
						}
					}
					Vector3 scaledSpacePointFromTA = patchRendering.GetScaledSpacePointFromTA(patchRendering.patch.TrueAnomalyAtUT(maneuverNode2.double_0), maneuverNode2.double_0);
					if ((bool)maneuverNode2.attachedGizmo)
					{
						Vector3d xzy = maneuverNode2.patch.getRelativePositionAtUT(maneuverNode2.double_0).xzy;
						Vector3d xzy2 = maneuverNode2.patch.getOrbitalVelocityAtUT(maneuverNode2.double_0).xzy;
						maneuverNode2.nodeRotation = QuaternionD.LookRotation(xzy2, Vector3d.Cross(-xzy, xzy2));
						maneuverNode2.attachedGizmo.transform.position = scaledSpacePointFromTA;
						maneuverNode2.attachedGizmo.transform.rotation = maneuverNode2.nodeRotation;
					}
					if (!maneuverNode2.scaledSpaceTarget)
					{
						maneuverNode2.scaledSpaceTarget = MapObject.Create("Maneuver #" + (solver.maneuverNodes.IndexOf(maneuverNode2) + 1), "Maneuver #" + (solver.maneuverNodes.IndexOf(maneuverNode2) + 1), maneuverNode2.patch, maneuverNode2);
						maneuverNode2.scaledSpaceTarget.vesselRef = vessel;
						MapView.MapCamera.AddTarget(maneuverNode2.scaledSpaceTarget);
					}
					maneuverNode2.scaledSpaceTarget.transform.position = scaledSpacePointFromTA;
					if (maneuverNode2.scaledSpaceTarget.uiNode == null)
					{
						MapNode.Create(maneuverNode2.scaledSpaceTarget, patchRendering.nodeColor, 32, hoverable: true, pinnable: true, blocksInput: true);
						maneuverNode2.scaledSpaceTarget.uiNode.OnUpdateVisible += ManeuverUINode_OnUpdateVisible;
						maneuverNode2.scaledSpaceTarget.uiNode.OnUpdateCaption += ManeuverUINode_OnUpdateCaption;
						maneuverNode2.scaledSpaceTarget.uiNode.OnUpdatePosition += ManeuverUINode_OnUpdatePosition;
						maneuverNode2.scaledSpaceTarget.uiNode.OnClick += ManeuverUINode_OnClick;
					}
					maneuverNode2.scaledSpaceTarget.uiNode.NodeUpdate();
					if (maneuverNode2.scaledSpaceTarget.uiNode.HoverOrPinned)
					{
						mouseOverNodes = true;
					}
				}
				else
				{
					maneuverNode2.DetachGizmo();
					if (maneuverNode2.scaledSpaceTarget != null && maneuverNode2.scaledSpaceTarget.uiNode != null)
					{
						maneuverNode2.scaledSpaceTarget.uiNode.Terminate();
					}
				}
			}
		}
		else
		{
			if (!renderEnabled)
			{
				return;
			}
			int m = 0;
			for (int count5 = patchRenders.Count; m < count5; m++)
			{
				if (patchRenders[m].enabled)
				{
					patchRenders[m].UpdateUINodes();
					patchRenders[m].DestroyVector();
				}
			}
			renderEnabled = false;
		}
	}

	public void ManeuverUINode_OnUpdateVisible(MapNode node, MapNode.IconData iData)
	{
		iData.visible = CanDrawAnyNode() && node.mapObject.maneuverNode.attachedGizmo == null;
	}

	public void ManeuverUINode_OnUpdateCaption(MapNode node, MapNode.CaptionData cData)
	{
		cData.Header = Localizer.Format("#autoLOC_198614");
		cData.captionLine1 = Localizer.Format("#autoLOC_198615", node.mapObject.maneuverNode.DeltaV.magnitude.ToString("0.0"));
		cData.captionLine2 = Localizer.Format("#autoLOC_198616", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - node.mapObject.maneuverNode.double_0, 3, explicitPositive: true));
	}

	public Vector3d ManeuverUINode_OnUpdatePosition(MapNode node)
	{
		return node.mapObject.transform.position;
	}

	public void ManeuverUINode_OnClick(MapNode mn, Mouse.Buttons btns)
	{
		if (HighLogic.LoadedSceneIsFlight && !ManeuverGizmoBase.HasMouseFocus && InputLockManager.IsUnlocked(ControlTypes.MAP_UI) && (btns & ~Mouse.Buttons.Left) == 0)
		{
			SetMouseOverGizmo(h: true);
			mn.mapObject.maneuverNode.AttachGizmo(MapView.ManeuverNodePrefab, this);
			mn.HideAddAlarm();
			GameEvents.onManeuverNodeSelected.Fire();
		}
	}

	public bool CanDrawAnyNode()
	{
		if (!Planetarium.Pause && Time.timeScale != 0f)
		{
			if (!MapView.MapIsEnabled)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
		if (solver != null)
		{
			for (int num = solver.maneuverNodes.Count - 1; num >= 0; num--)
			{
				solver.maneuverNodes[num].RemoveSelf();
			}
		}
		if (patchRenders != null)
		{
			int i = 0;
			for (int count = patchRenders.Count; i < count; i++)
			{
				patchRenders[i].Terminate();
			}
			int j = 0;
			for (int count2 = flightPlanRenders.Count; j < count2; j++)
			{
				flightPlanRenders[j].Terminate();
			}
		}
	}

	public void OnSceneSwitch(GameScenes scn)
	{
		for (int num = solver.maneuverNodes.Count - 1; num >= 0; num--)
		{
			solver.maneuverNodes[num].RemoveSelf();
		}
	}

	public PatchRendering FindRenderingForPatch(Orbit patch)
	{
		int count = flightPlanRenders.Count;
		int num = 0;
		PatchRendering patchRendering;
		while (true)
		{
			if (num < count)
			{
				patchRendering = flightPlanRenders[num];
				if (patchRendering.patch == patch)
				{
					break;
				}
				num++;
				continue;
			}
			count = patchRenders.Count;
			num = 0;
			while (true)
			{
				if (num < count)
				{
					patchRendering = patchRenders[num];
					if (patchRendering.patch == patch)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return patchRendering;
		}
		return patchRendering;
	}

	public void AddManeuverNode(double double_0)
	{
		ManeuverNode node = solver.AddManeuverNode(double_0);
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			node.AttachGizmo(MapView.ManeuverNodePrefab, this);
			GameEvents.onManeuverNodeSelected.Fire();
		}));
	}

	public void SetMouseOverGizmo(bool h)
	{
		if (h)
		{
			mouseOverGizmos = true;
		}
		else
		{
			if (mouseOverNodes)
			{
				return;
			}
			int count = solver.maneuverNodes.Count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					ManeuverNode maneuverNode = solver.maneuverNodes[num];
					if (!maneuverNode.attachedGizmo || !maneuverNode.attachedGizmo.MouseOverGizmo)
					{
						num++;
						continue;
					}
					break;
				}
				mouseOverGizmos = false;
				break;
			}
		}
	}
}
