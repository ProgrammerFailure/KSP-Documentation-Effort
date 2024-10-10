using System;
using UnityEngine;

[Serializable]
public class ManeuverNode : IConfigNode
{
	public double double_0;

	public Vector3d DeltaV;

	public Quaternion nodeRotation;

	public ManeuverGizmo attachedGizmo;

	public PatchedConicSolver solver;

	public MapObject scaledSpaceTarget;

	public Orbit patch;

	public Orbit nextPatch;

	public bool refocusCamera = true;

	public double startBurnIn { get; set; }

	public void AttachGizmo(GameObject gizmoPrefab, PatchedConicRenderer renderer)
	{
		attachedGizmo = UnityEngine.Object.Instantiate(gizmoPrefab).GetComponent<ManeuverGizmo>();
		attachedGizmo.gameObject.SetActive(value: true);
		attachedGizmo.name = "Maneuver Node";
		attachedGizmo.DeltaV = DeltaV;
		attachedGizmo.double_0 = double_0;
		attachedGizmo.OnGizmoUpdated = OnGizmoUpdated;
		attachedGizmo.OnMinimize = DetachGizmo;
		attachedGizmo.OnDelete = RemoveSelf;
		attachedGizmo.Setup(this, renderer);
	}

	public void OnGizmoUpdated(Vector3d dV, double ut)
	{
		Vector3d deltaV = DeltaV;
		double num = double_0;
		DeltaV = dV;
		double_0 = ut;
		solver.UpdateFlightPlan();
		if (attachedGizmo == null)
		{
			return;
		}
		if (patch == null)
		{
			DeltaV = deltaV;
			double_0 = num;
			solver.UpdateFlightPlan();
			return;
		}
		bool reset = false;
		if (refocusCamera && (attachedGizmo.patchBefore.referenceBody.name != patch.referenceBody.name || Math.Abs(attachedGizmo.patchBefore.StartUT - patch.StartUT) > 100.0))
		{
			if (PlanetariumCamera.fetch != null && patch.referenceBody.MapObject != null)
			{
				PlanetariumCamera.fetch.SetTarget(patch.referenceBody.MapObject);
			}
			reset = true;
		}
		attachedGizmo.SetPatches(patch, nextPatch, reset);
	}

	public void DetachGizmo()
	{
		if ((bool)attachedGizmo)
		{
			attachedGizmo.Terminate();
			attachedGizmo = null;
			GameEvents.onManeuverNodeDeselected.Fire();
		}
	}

	public void RemoveSelf()
	{
		DetachGizmo();
		if ((bool)scaledSpaceTarget)
		{
			MapView.MapCamera.RemoveTarget(scaledSpaceTarget);
			scaledSpaceTarget.Terminate();
		}
		solver.RemoveManeuverNode(this);
	}

	public Vector3d GetBurnVector(Orbit currentOrbit)
	{
		if (currentOrbit != null && nextPatch != null)
		{
			if (currentOrbit.referenceBody != nextPatch.referenceBody)
			{
				return (nextPatch.getOrbitalVelocityAtUT(double_0) - patch.getOrbitalVelocityAtUT(double_0)).xzy;
			}
			return (nextPatch.getOrbitalVelocityAtUT(double_0) - currentOrbit.getOrbitalVelocityAtUT(double_0)).xzy;
		}
		return Vector3d.zero;
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("UT"))
		{
			double_0 = double.Parse(node.GetValue("UT"));
		}
		if (node.HasValue("dV"))
		{
			DeltaV = KSPUtil.ParseVector3d(node.GetValue("dV"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("UT", double_0);
		node.AddValue("dV", KSPUtil.WriteVector(DeltaV));
	}

	public Vector3d GetPartialDv()
	{
		if (patch != null && nextPatch != null)
		{
			Vector3d vector3d = nextPatch.getOrbitalVelocityAtUT(double_0) - patch.getOrbitalVelocityAtUT(double_0);
			Vector3d normalized = patch.getOrbitalVelocityAtUT(double_0).normalized;
			Vector3d normalized2 = patch.GetOrbitNormal().normalized;
			return QuaternionD.Inverse(QuaternionD.LookRotation(normalized, normalized2)) * vector3d;
		}
		return Vector3d.zero;
	}
}
