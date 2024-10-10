using System;
using System.Collections;
using System.Collections.Generic;
using ns9;
using UnityEngine;

[RequireComponent(typeof(OrbitDriver))]
public class PatchedConicSolver : MonoBehaviour
{
	public OrbitDriver obtDriver;

	public int patchLimit = 2;

	public int maxTotalPatches = 150;

	public List<Orbit> patches;

	public List<Orbit> flightPlan;

	public List<ManeuverNode> maneuverNodes;

	public bool MorePatchesAhead;

	public int patchesAhead;

	public int maxGeometrySolverIterations = 25;

	public int maxTimeSolverIterations = 50;

	public int GeoSolverIterations;

	public int TimeSolverIterations1;

	public int TimeSolverIterations2;

	public bool debug_disableEscapeCheck;

	public double outerReaches = 1E+20;

	public PatchedConics.SolverParameters parameters;

	public CelestialBody targetBody;

	public List<ManeuverNode> flightPlanLoadList;

	public bool updateFlightPlan = true;

	public Orbit orbit => obtDriver.orbit;

	public Orbit LastActivePatch
	{
		get
		{
			Orbit result = patches[0];
			for (int i = 0; i < patches.Count; i++)
			{
				Orbit orbit = patches[i];
				if (!orbit.activePatch)
				{
					break;
				}
				result = orbit;
			}
			return result;
		}
	}

	public void Awake()
	{
		obtDriver = GetComponent<OrbitDriver>();
		OrbitDriver orbitDriver = obtDriver;
		orbitDriver.OnReferenceBodyChange = (OrbitDriver.CelestialBodyDelegate)Delegate.Combine(orbitDriver.OnReferenceBodyChange, new OrbitDriver.CelestialBodyDelegate(OnReferenceBodyChange));
		patches = new List<Orbit>();
		flightPlan = new List<Orbit>();
		flightPlanLoadList = new List<ManeuverNode>();
		maneuverNodes = new List<ManeuverNode>();
		patchLimit = Mathf.Max(GameSettings.CONIC_PATCH_LIMIT, 1);
		for (int i = 0; i < maxTotalPatches; i++)
		{
			if (i == 0)
			{
				patches.Add(obtDriver.orbit);
			}
			else
			{
				patches.Add(new Orbit());
			}
		}
		parameters = new PatchedConics.SolverParameters();
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
	}

	public void OnDestroy()
	{
		if (obtDriver != null)
		{
			OrbitDriver orbitDriver = obtDriver;
			orbitDriver.OnReferenceBodyChange = (OrbitDriver.CelestialBodyDelegate)Delegate.Remove(orbitDriver.OnReferenceBodyChange, new OrbitDriver.CelestialBodyDelegate(OnReferenceBodyChange));
		}
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
	}

	public void OnReferenceBodyChange(CelestialBody body)
	{
		updateFlightPlan = true;
	}

	public void OnGameSettingsApplied()
	{
		if (patchLimit != GameSettings.CONIC_PATCH_LIMIT)
		{
			patchLimit = Mathf.Max(GameSettings.CONIC_PATCH_LIMIT, 1);
			UpdateFlightPlan();
		}
	}

	public IEnumerator Start()
	{
		int count = flightPlanLoadList.Count;
		int i = 0;
		while (i < count)
		{
			yield return null;
			ManeuverNode maneuverNode = flightPlanLoadList[i];
			maneuverNode.solver = this;
			maneuverNodes.Add(maneuverNode);
			int num = i + 1;
			i = num;
		}
		if (count > 0)
		{
			UpdateFlightPlan();
			if (obtDriver.vessel != null)
			{
				GameEvents.onManeuverAdded.Fire(obtDriver.vessel, this);
				GameEvents.onManeuversLoaded.Fire(obtDriver.vessel, this);
			}
		}
	}

	public void Update()
	{
		if (Planetarium.Pause || Time.timeScale == 0f)
		{
			return;
		}
		patchLimit = Mathf.Max(Mathf.Min(maxTotalPatches - 1, patchLimit), 1);
		for (int i = 0; i < patches.Count; i++)
		{
			patches[i].activePatch = false;
		}
		if (obtDriver.updateMode != OrbitDriver.UpdateMode.IDLE)
		{
			patches[0] = orbit;
			patches[0].patchStartTransition = Orbit.PatchTransitionType.INITIAL;
			patches[0].StartUT = Planetarium.GetUniversalTime();
			if (patches[0].eccentricity < 1.0)
			{
				patches[0].EndUT = patches[0].StartUT + patches[0].period;
			}
			else
			{
				patches[0].EndUT = patches[0].period;
			}
			int num = 0;
			do
			{
				MorePatchesAhead = PatchedConics.CalculatePatch(patches[num], patches[num + 1], (num != 0) ? patches[num].epoch : Planetarium.GetUniversalTime(), parameters, targetBody);
				patchesAhead = num;
				num++;
			}
			while (MorePatchesAhead && (num < patchLimit || (maneuverNodes.Count > 0 && FindPatchContainingUT(maneuverNodes[0].double_0) == null)));
			if (updateFlightPlan)
			{
				UpdateFlightPlan();
				updateFlightPlan = false;
			}
		}
	}

	public void UpdateFlightPlan()
	{
		if (maneuverNodes.Count <= 0)
		{
			return;
		}
		maneuverNodes.Sort(SortNodesByDate);
		flightPlan.Clear();
		Orbit orbit = FindPatchContainingUT(maneuverNodes[0].double_0);
		if (orbit == null)
		{
			if (maneuverNodes[0].double_0 < Planetarium.GetUniversalTime())
			{
				Debug.LogWarning("Maneuver Node: removed unsolveable stale Maneuver Node");
				ScreenMessages.PostScreenMessage("<color=orange><b>" + Localizer.Format("#autoLOC_6002379") + "</b></color>", 15f);
				maneuverNodes[0].RemoveSelf();
			}
			return;
		}
		for (int i = 0; i < patches.Count && patches[i] != orbit; i++)
		{
			flightPlan.Add(patches[i]);
		}
		CheckNextManeuver(0, orbit, 1);
		if ((bool)FlightUIModeController.Instance && (FlightUIModeController.Instance.Mode == FlightUIMode.MANEUVER_EDIT || FlightUIModeController.Instance.Mode == FlightUIMode.MANEUVER_INFO))
		{
			ManeuverNodeEditorManager.Instance.SetManeuverNodeInitialValues();
		}
	}

	public Orbit FindPatchContainingUT(double double_0)
	{
		double universalTime = Planetarium.GetUniversalTime();
		for (int i = 0; i < patches.Count; i++)
		{
			Orbit orbit = patches[i];
			if (!orbit.activePatch)
			{
				break;
			}
			if ((double_0 < universalTime && double_0 <= orbit.StartUT) || (double_0 >= orbit.StartUT && double_0 <= orbit.EndUT) || orbit.patchEndTransition == Orbit.PatchTransitionType.FINAL)
			{
				return orbit;
			}
		}
		return null;
	}

	public int FindManeuverIndex(ManeuverNode node)
	{
		int num = 0;
		while (true)
		{
			if (num < maneuverNodes.Count)
			{
				if (maneuverNodes[num].double_0.Equals(node.double_0))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public void CheckNextManeuver(int nodeIdx, Orbit nodePatch, int patchesAhead)
	{
		flightPlan.Add(nodePatch);
		double universalTime = Planetarium.GetUniversalTime();
		Orbit orbit = new Orbit();
		bool flag = false;
		if (nodeIdx != 0)
		{
			flag = PatchedConics.CalculatePatch(nodePatch, orbit, nodePatch.epoch, parameters, targetBody);
		}
		if (nodeIdx < maneuverNodes.Count)
		{
			ManeuverNode maneuverNode = maneuverNodes[nodeIdx];
			if ((maneuverNode.double_0 < universalTime && maneuverNode.double_0 <= nodePatch.StartUT) || (maneuverNode.double_0 > nodePatch.StartUT && maneuverNode.double_0 < nodePatch.EndUT) || nodePatch.patchEndTransition == Orbit.PatchTransitionType.FINAL)
			{
				maneuverNode.patch = nodePatch;
				nodePatch.GetOrbitalStateVectorsAtUT(maneuverNode.double_0, out var state);
				Vector3d xzy = state.pos.xzy;
				Vector3d xzy2 = state.vel.xzy;
				Vector3d vector3d = QuaternionD.LookRotation(xzy2, Vector3d.Cross(-xzy, xzy2)) * maneuverNode.DeltaV;
				maneuverNode.patch = nodePatch;
				maneuverNode.nextPatch = orbit;
				orbit.previousPatch = nodePatch;
				orbit.UpdateFromFixedVectors(xzy.xzy, xzy2.xzy + vector3d.xzy, nodePatch.referenceBody, maneuverNode.double_0);
				orbit.patchStartTransition = Orbit.PatchTransitionType.MANEUVER;
				orbit.StartUT = maneuverNode.double_0;
				orbit.EndUT = ((orbit.eccentricity < 1.0) ? (maneuverNode.double_0 + orbit.period) : orbit.period);
				orbit.DrawOrbit();
				if (nodeIdx != 0)
				{
					nodePatch.patchEndTransition = Orbit.PatchTransitionType.MANEUVER;
					nodePatch.EndUT = maneuverNode.double_0;
					nodePatch.activePatch = true;
				}
				CheckNextManeuver(nodeIdx + 1, orbit, 1);
				return;
			}
		}
		if (flag && (patchesAhead < patchLimit || nodeIdx < maneuverNodes.Count))
		{
			CheckNextManeuver(nodeIdx, orbit, patchesAhead + 1);
		}
	}

	public ManeuverNode AddManeuverNode(double double_0)
	{
		return AddManeuverNode(double_0, updateFlightPlan: true);
	}

	public ManeuverNode AddManeuverNode(double double_0, bool updateFlightPlan)
	{
		ManeuverNode maneuverNode = new ManeuverNode();
		maneuverNode.solver = this;
		maneuverNode.DeltaV = Vector3d.zero;
		maneuverNode.double_0 = double_0;
		maneuverNodes.Add(maneuverNode);
		if (updateFlightPlan)
		{
			UpdateFlightPlan();
			if (obtDriver.vessel != null)
			{
				GameEvents.onManeuverAdded.Fire(obtDriver.vessel, this);
			}
		}
		return maneuverNode;
	}

	public void RemoveManeuverNode(ManeuverNode node)
	{
		if (!maneuverNodes.Contains(node))
		{
			Debug.LogWarning("Patched Conics Solver: Cannot remove a node not found on the nodes list", base.gameObject);
			return;
		}
		maneuverNodes.Remove(node);
		if (maneuverNodes.Count == 0)
		{
			flightPlan.Clear();
		}
		else
		{
			Update();
			UpdateFlightPlan();
		}
		if (obtDriver.vessel != null)
		{
			GameEvents.onManeuverRemoved.Fire(obtDriver.vessel, this);
		}
	}

	public int SortNodesByDate(ManeuverNode m1, ManeuverNode m2)
	{
		return m1.double_0.CompareTo(m2.double_0);
	}

	[ContextMenu("Increase Patch Limit")]
	public void IncreasePatchLimit()
	{
		patchLimit++;
		UpdateFlightPlan();
	}

	[ContextMenu("Decrease Patch Limit")]
	public void DecreasePatchLimit()
	{
		patchLimit = Math.Max(1, patchLimit - 1);
		UpdateFlightPlan();
	}

	public void Load(ConfigNode node)
	{
		ConfigNode[] nodes = node.GetNodes("MANEUVER");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ManeuverNode maneuverNode = new ManeuverNode();
			maneuverNode.Load(nodes[i]);
			maneuverNode.solver = this;
			flightPlanLoadList.Add(maneuverNode);
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < maneuverNodes.Count; i++)
		{
			maneuverNodes[i].Save(node.AddNode("MANEUVER"));
		}
	}
}
