using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveJoint : IConfigNode
{
	public enum JointMode
	{
		Motor,
		Pivot,
		Piston
	}

	public enum DriveMode
	{
		NoJoint,
		Park,
		Neutral,
		Drive
	}

	public struct Coords
	{
		public Vector3 position;

		public Quaternion rotation;

		public Coords(Vector3 pos, Quaternion rot)
		{
			position = pos;
			rotation = rot;
		}
	}

	public AttachNode refNode;

	public bool targetParent;

	public Vector3 dPos;

	public Vector3 lastDPos;

	public Quaternion dRot;

	public Vector3 axis;

	public Vector3 secAxis;

	public Vector3 anchor;

	public Vector3 pivot;

	public Vector3 lastPivot;

	public Vector3 ctrlAxis;

	public Vector3 activeAxis;

	public Vector3 initOrt;

	public Vector3 lastOrt;

	public Vector3 endOrt;

	public float Angle;

	public float lastAngle;

	public Quaternion jointRot0;

	public Quaternion lastDRot;

	public float maxJointDamper = 1E+20f;

	public JointDrive targetDrive;

	public IActiveJointHost moduleHost;

	public Part hostPart;

	public Vector3 dInitOrt;

	public Vector3 dEndOrt;

	public Vector3 dCrossVector;

	public Vector3 jointSpaceAxis;

	public PartJoint pJoint { get; set; }

	public ConfigurableJoint joint { get; set; }

	public JointMode jointMode { get; set; }

	public DriveMode driveMode { get; set; }

	public bool isValid => hostPart != null;

	public static ActiveJoint Create(IActiveJointHost moduleHost, string refNodeId, JointMode function)
	{
		ActiveJoint activeJoint = new ActiveJoint();
		activeJoint.moduleHost = moduleHost;
		activeJoint.jointMode = function;
		activeJoint.startForRefNode(refNodeId);
		return activeJoint;
	}

	public static ActiveJoint Create(IActiveJointHost moduleHost, AttachNode refNode, JointMode function)
	{
		ActiveJoint activeJoint = new ActiveJoint();
		activeJoint.moduleHost = moduleHost;
		activeJoint.jointMode = function;
		activeJoint.refNode = refNode;
		activeJoint.startForRefNode(refNode.id);
		return activeJoint;
	}

	public void Terminate()
	{
		GameEvents.onPartDie.Remove(onPartDestroyed);
		GameEvents.onPartDestroyed.Remove(onPartDestroyed);
		GameEvents.onPartJointBreak.Remove(onPartJointBreak);
		GameEvents.onPartPack.Remove(onPartPack);
		GameEvents.onVesselWasModified.Remove(onVesselModified);
		GameEvents.onActiveJointNeedUpdate.Remove(onActiveJointNeedUpdate);
		try
		{
			if (driveMode != 0)
			{
				SetDriveMode(DriveMode.Park);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("ActiveJoint.Terminate Exception while setting drive mode: " + ex);
		}
		hostPart = null;
		moduleHost = null;
		pJoint = null;
		joint = null;
	}

	public void startForRefNode(string jointNodeName)
	{
		driveMode = DriveMode.NoJoint;
		hostPart = moduleHost.GetHostPart();
		if (refNode != null)
		{
			targetParent = false;
		}
		else if (jointNodeName != string.Empty)
		{
			refNode = hostPart.FindAttachNode(jointNodeName);
			targetParent = false;
		}
		else
		{
			refNode = null;
			targetParent = true;
		}
		if (jointNodeName != string.Empty && refNode == null)
		{
			Debug.LogError("[ActiveJoint]: Cannot initialize, no attachment node found with id " + jointNodeName, hostPart.gameObject);
			return;
		}
		if (targetParent)
		{
			if (hostPart.parent == null)
			{
				Debug.LogError("[ActiveJoint]: Cannot initialize, parent part is null!", hostPart.gameObject);
				return;
			}
			if (hostPart.parent == hostPart)
			{
				Debug.LogError("[ActiveJoint]: Cannot initialize, host part and target part cannot be the same.", hostPart.gameObject);
				return;
			}
		}
		GameEvents.onPartDie.Add(onPartDestroyed);
		GameEvents.onPartDestroyed.Add(onPartDestroyed);
		if (HighLogic.LoadedSceneIsFlight)
		{
			hostPart.StartCoroutine(startAfterJointsCreated());
		}
	}

	public IEnumerator startAfterJointsCreated()
	{
		while (!hostPart.started)
		{
			yield return null;
		}
		InitJoint();
	}

	public void restartJoint()
	{
		if (hostPart != null && hostPart.State != PartStates.DEAD)
		{
			hostPart.StartCoroutine(waitAndRestart());
		}
		else
		{
			Terminate();
		}
	}

	public IEnumerator waitAndRestart()
	{
		yield return null;
		if (hostPart != null)
		{
			hostPart = moduleHost.GetHostPart();
			InitJoint();
		}
	}

	public void InitJoint()
	{
		if (refNode != null)
		{
			pJoint = findJointAtNode(refNode);
		}
		else if (targetParent)
		{
			pJoint = findJointBetweenParts(hostPart, hostPart.parent);
		}
		if (pJoint != null)
		{
			joint = pJoint.Joint;
			axis = joint.axis;
			secAxis = joint.secondaryAxis;
			Angle = 0f;
			lastAngle = 0f;
			jointRot0 = Quaternion.identity;
			targetDrive = GetJointDrive();
			initOrt = getControlOrt(GetCtrlAxis(), PartSpaceMode.Pristine);
			lastOrt = initOrt;
			dPos = Vector3.zero;
			lastDPos = Vector3.zero;
			dRot = Quaternion.identity;
			lastDRot = Quaternion.identity;
			if (pJoint.Host == pJoint.Child)
			{
				pivot = Part.PartToVesselSpacePos(pJoint.HostAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
			else
			{
				pivot = Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Child, pJoint.Child.vessel, PartSpaceMode.Pristine);
			}
			GameEvents.onPartJointBreak.Add(onPartJointBreak);
			GameEvents.onPartPack.Add(onPartPack);
			GameEvents.onActiveJointNeedUpdate.Add(onActiveJointNeedUpdate);
			driveMode = DriveMode.Park;
			onJointInit(jointExists: true);
			moduleHost.OnJointInit(this);
		}
		else
		{
			GameEvents.onVesselWasModified.Add(onVesselModified);
			onJointInit(jointExists: false);
			moduleHost.OnJointInit(null);
		}
	}

	public PartJoint findJointAtNode(AttachNode node)
	{
		if (node.attachedPart != null)
		{
			if (node.attachedPart == hostPart.parent)
			{
				return hostPart.attachJoint;
			}
			if (node.owner.physicalSignificance != 0)
			{
				return findAttachJointForTargetPart(node.attachedPart, hostPart.Rigidbody);
			}
			return node.owner.attachJoint;
		}
		return null;
	}

	public PartJoint findJointBetweenParts(Part p1, Part p2)
	{
		if (p1.parent == p2)
		{
			return p1.attachJoint;
		}
		if (p2.parent == p1)
		{
			return p2.attachJoint;
		}
		return null;
	}

	public PartJoint findAttachJointForTargetPart(Part p, Rigidbody connectedBody)
	{
		int num = 0;
		PartJoint partJoint;
		while (true)
		{
			if (num < p.children.Count)
			{
				Part part = p.children[num];
				if (!(part.attachJoint != null) || !(part.attachJoint.Joint.connectedBody == connectedBody))
				{
					partJoint = findAttachJointForTargetPart(part, connectedBody);
					if (partJoint != null)
					{
						break;
					}
					num++;
					continue;
				}
				return part.attachJoint;
			}
			return null;
		}
		return partJoint;
	}

	public JointDrive GetJointDrive()
	{
		return jointMode switch
		{
			JointMode.Pivot => pJoint.Joint.angularYZDrive, 
			JointMode.Piston => pJoint.Joint.xDrive, 
			_ => pJoint.Joint.angularXDrive, 
		};
	}

	public void SetJointDrive(JointDrive drive)
	{
		switch (jointMode)
		{
		default:
			pJoint.Joint.angularXDrive = drive;
			break;
		case JointMode.Pivot:
			pJoint.Joint.angularYZDrive = drive;
			break;
		case JointMode.Piston:
			pJoint.Joint.xDrive = drive;
			break;
		}
	}

	public bool SetDriveMode(DriveMode m)
	{
		if (m != 0 && joint == null)
		{
			return false;
		}
		switch (m)
		{
		case DriveMode.NoJoint:
			GameEvents.onPartJointBreak.Remove(onPartJointBreak);
			GameEvents.onPartPack.Remove(onPartPack);
			GameEvents.onActiveJointNeedUpdate.Remove(onActiveJointNeedUpdate);
			break;
		case DriveMode.Park:
			if (jointMode == JointMode.Piston)
			{
				targetDrive.positionDamper = maxJointDamper;
				targetDrive.positionSpring = pJoint.stiffness;
			}
			else
			{
				targetDrive.positionDamper = 0f;
				targetDrive.positionSpring = pJoint.stiffness;
			}
			SetJointDrive(targetDrive);
			applyCoordsUpdate();
			break;
		case DriveMode.Neutral:
			targetDrive.positionDamper = 0f;
			targetDrive.positionSpring = 0f;
			SetJointDrive(targetDrive);
			break;
		case DriveMode.Drive:
			targetDrive.positionDamper = maxJointDamper;
			targetDrive.positionSpring = 0f;
			SetJointDrive(targetDrive);
			break;
		}
		Debug.DrawRay(pJoint.Host.partTransform.TransformPoint(pJoint.HostAnchor), pJoint.Target.partTransform.rotation * initOrt, Color.green, 5f);
		if (m != driveMode)
		{
			driveMode = m;
			moduleHost.OnDriveModeChanged(m);
		}
		return true;
	}

	public void onPartDestroyed(Part p)
	{
		if (p == hostPart)
		{
			Terminate();
		}
	}

	public void onVesselModified(Vessel v)
	{
		if (!(hostPart == null) && hostPart.State != PartStates.DEAD)
		{
			if (v == hostPart.vessel)
			{
				if (driveMode == DriveMode.NoJoint)
				{
					GameEvents.onVesselWasModified.Remove(onVesselModified);
					restartJoint();
				}
				else
				{
					applyCoordsUpdate();
				}
			}
		}
		else
		{
			Debug.Log("[ActiveJoint]: host part is null, active joint must terminate.");
			Terminate();
		}
	}

	public void onPartJointBreak(PartJoint pj, float force)
	{
		if (driveMode != 0 && pJoint == pj)
		{
			SetDriveMode(DriveMode.NoJoint);
			restartJoint();
		}
	}

	public void onPartPack(Part p)
	{
		if (p == pJoint.Host)
		{
			if (!(targetDrive.positionSpring > 0f))
			{
				SetDriveMode(DriveMode.Park);
			}
		}
		else
		{
			applyCoordsUpdate();
		}
	}

	public void onActiveJointNeedUpdate(Vessel v)
	{
		if (v == hostPart.vessel)
		{
			applyCoordsUpdate();
		}
	}

	public Vector3 GetCtrlAxis()
	{
		return jointMode switch
		{
			JointMode.Pivot => axis, 
			_ => secAxis, 
		};
	}

	public Vector3 getControlOrt(Vector3 refAxis, PartSpaceMode mode)
	{
		if (mode != 0 && mode == PartSpaceMode.Current)
		{
			return pJoint.Target.partTransform.InverseTransformDirection(pJoint.Host.partTransform.TransformDirection(refAxis));
		}
		return Part.VesselToPartSpaceDir(Part.PartToVesselSpaceDir(refAxis, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Pristine), pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine);
	}

	public Vector3 getInvControlOrt(Vector3 refAxis, PartSpaceMode mode)
	{
		if (mode != 0 && mode == PartSpaceMode.Current)
		{
			return pJoint.Host.partTransform.InverseTransformDirection(pJoint.Target.partTransform.TransformDirection(refAxis));
		}
		return Part.VesselToPartSpaceDir(Part.PartToVesselSpaceDir(refAxis, pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine), pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Pristine);
	}

	public Vector3 getControlPos(Vector3 refPos, PartSpaceMode mode)
	{
		if (mode != 0 && mode == PartSpaceMode.Current)
		{
			return pJoint.Target.partTransform.InverseTransformPoint(pJoint.Host.partTransform.TransformPoint(refPos));
		}
		return Part.VesselToPartSpacePos(Part.PartToVesselSpacePos(refPos, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Pristine), pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine);
	}

	public Vector3 getInvControlPos(Vector3 refPos, PartSpaceMode mode)
	{
		if (mode != 0 && mode == PartSpaceMode.Current)
		{
			return pJoint.Host.partTransform.InverseTransformPoint(pJoint.Target.partTransform.TransformPoint(refPos));
		}
		return Part.VesselToPartSpacePos(Part.PartToVesselSpacePos(refPos, pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine), pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Pristine);
	}

	public void applyCoordsUpdate()
	{
		endOrt = getControlOrt(GetCtrlAxis(), PartSpaceMode.Current);
		switch (jointMode)
		{
		case JointMode.Pivot:
			activeAxis = Vector3.Cross(initOrt, endOrt).normalized;
			Angle = Mathf.Acos(Vector3.Dot(initOrt, endOrt)) * 57.29578f;
			if (float.IsNaN(Angle) || Angle == 0f)
			{
				break;
			}
			joint.SetTargetRotationLocal(Quaternion.AngleAxis(Angle, getInvControlOrt(activeAxis, PartSpaceMode.Current)), jointRot0);
			if (pJoint.Child == pJoint.Host)
			{
				activeAxis = getInvControlOrt(activeAxis, PartSpaceMode.Current);
				dRot = Quaternion.AngleAxis(Angle, Part.PartToVesselSpaceDir(activeAxis, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Current).normalized);
				if (pJoint.Parent.vessel != null && pJoint.Parent.vessel.transform != null)
				{
					pivot = Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Parent, pJoint.Parent.vessel, PartSpaceMode.Pristine) + getAnchorOffset(PartSpaceMode.Pristine);
				}
				DebugDrawUtil.DrawCrosshairs(hostPart.vessel.transform.TransformPoint(pivot), 1f, Color.green, 5f);
			}
			else
			{
				activeAxis = getInvControlOrt(activeAxis, PartSpaceMode.Current);
				activeAxis *= -1f;
				dRot = Quaternion.AngleAxis(Angle, Part.PartToVesselSpaceDir(activeAxis, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Current).normalized);
				if (pJoint.Target.vessel != null && pJoint.Target.vessel.transform != null)
				{
					pivot = Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine) + getAnchorOffset(PartSpaceMode.Pristine);
				}
				DebugDrawUtil.DrawCrosshairs(hostPart.vessel.transform.TransformPoint(pivot), 1f, Color.yellow, 5f);
			}
			recurseCoordUpdate(pJoint.Child, -lastDPos, Quaternion.Inverse(lastDRot), lastPivot);
			recurseCoordUpdate(pJoint.Child, dPos, dRot, pivot);
			lastDPos = dPos;
			lastDRot = dRot;
			lastPivot = pivot;
			break;
		default:
			activeAxis = pJoint.Axis;
			Angle = KSPUtil.HeadingDegrees(initOrt, endOrt, getControlOrt(activeAxis, PartSpaceMode.Current));
			if (!float.IsNaN(Angle) && Angle - lastAngle != 0f)
			{
				joint.SetTargetRotationLocal(Quaternion.AngleAxis(Angle, activeAxis), jointRot0);
				activeAxis = ((pJoint.Child == pJoint.Host) ? activeAxis : (-activeAxis));
				dRot = Quaternion.AngleAxis(Angle - lastAngle, Quaternion.Inverse(pJoint.Host.vessel.transform.rotation) * pJoint.Host.partTransform.rotation * activeAxis);
				lastAngle = Angle;
				Angle = 0f;
				if (pJoint.Host.vessel != null && pJoint.Host.vessel.transform != null)
				{
					recurseCoordUpdate(pJoint.Child, dPos, dRot, Part.PartToVesselSpacePos(pJoint.HostAnchor, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Current));
				}
			}
			break;
		}
		dRot = Quaternion.identity;
		dPos.Zero();
		lastOrt = endOrt;
	}

	public void recurseCoordUpdate(Part p, Vector3 dPos, Quaternion dRot, Vector3 pivot)
	{
		p.orgPos = dRot * (p.orgPos - pivot) + pivot + dRot * dPos;
		p.orgRot = dRot * p.orgRot;
		for (int i = 0; i < p.children.Count; i++)
		{
			recurseCoordUpdate(p.children[i], dPos, dRot, pivot);
		}
	}

	public void SetAxis(Transform srcSpace, Vector3 axis)
	{
		joint.axis = pJoint.Host.partTransform.InverseTransformDirection(srcSpace.TransformDirection(axis));
		this.axis = joint.axis;
	}

	public void SetSecondaryAxis(Transform srcSpace, Vector3 secondAxis)
	{
		joint.secondaryAxis = pJoint.Host.partTransform.InverseTransformDirection(srcSpace.TransformDirection(secondAxis));
		secAxis = joint.secondaryAxis;
	}

	public void Load(ConfigNode node)
	{
	}

	public void Save(ConfigNode node)
	{
	}

	public Vector3 getAnchorOffset(PartSpaceMode mode)
	{
		if (mode != 0 && mode == PartSpaceMode.Current)
		{
			return hostPart.vessel.transform.InverseTransformPoint(pJoint.Host.partTransform.TransformPoint(pJoint.HostAnchor) - pJoint.Target.partTransform.TransformPoint(pJoint.TgtAnchor));
		}
		return Part.PartToVesselSpacePos(pJoint.HostAnchor, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Pristine) - Part.PartToVesselSpacePos(pJoint.TgtAnchor, pJoint.Target, pJoint.Target.vessel, PartSpaceMode.Pristine);
	}

	public virtual void onJointInit(bool jointExists)
	{
	}

	public void DrawDebug()
	{
		if (driveMode != 0)
		{
			dInitOrt = pJoint.Target.partTransform.rotation * initOrt;
			dEndOrt = pJoint.Target.partTransform.rotation * getControlOrt(GetCtrlAxis(), PartSpaceMode.Pristine);
			dCrossVector = Vector3.Cross(dInitOrt, dEndOrt).normalized;
			Angle = KSPUtil.HeadingDegrees(dInitOrt, dEndOrt, activeAxis);
			Debug.DrawRay(pJoint.Target.partTransform.position, dInitOrt, Color.green);
			Debug.DrawRay(pJoint.Target.partTransform.position, dEndOrt, Color.cyan);
			Debug.DrawRay(pJoint.Target.partTransform.position, dCrossVector, Color.magenta);
			jointSpaceAxis = pJoint.LocaltoJointSpaceDir(getInvControlOrt(dCrossVector, PartSpaceMode.Pristine));
			Debug.DrawRay(pJoint.Host.partTransform.TransformPoint(pJoint.JointToLocalSpacePos(pJoint.HostAnchor)), pJoint.Host.partTransform.TransformDirection(pJoint.JointToLocalSpaceDir(jointSpaceAxis)), Color.blue);
		}
	}

	public List<Coords> storeVesselCoords()
	{
		List<Coords> list = new List<Coords>();
		for (int i = 0; i < hostPart.vessel.parts.Count; i++)
		{
			Part part = hostPart.vessel.parts[i];
			if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				list.Add(new Coords(part.partTransform.position, part.partTransform.rotation));
			}
		}
		return list;
	}

	public void restoreVesselCoords(List<Coords> storedCoords)
	{
		for (int i = 0; i < hostPart.vessel.parts.Count; i++)
		{
			Part part = hostPart.vessel.parts[i];
			if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				part.partTransform.position = storedCoords[i].position;
				part.partTransform.rotation = storedCoords[i].rotation;
			}
		}
	}
}
