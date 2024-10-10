using System.Collections;
using UnityEngine;

public abstract class ModuleJointMotor : PartModule, IJointLockState
{
	public enum Mode
	{
		NoJoint,
		Park,
		Neutral,
		Drive
	}

	[KSPField]
	public string jointNodeName = "";

	public Vector3 dPos;

	public Quaternion dRot;

	public Vector3 axis;

	public Vector3 secAxis;

	public JointDrive angXDrive;

	public Vector3 initOrt;

	public Vector3 endOrt;

	public Vector3 ctrlAxis;

	public float xAngle;

	public float lastXAngle;

	public Mode mode;

	public AttachNode refNode { get; set; }

	public PartJoint pJoint { get; set; }

	public ConfigurableJoint joint { get; set; }

	public ModuleJointMotor()
	{
	}

	public override void OnLoad(ConfigNode node)
	{
		OnModuleLoad(node);
	}

	public override void OnSave(ConfigNode node)
	{
		if (pJoint != null)
		{
			applyCoordsUpdate();
		}
		OnModuleSave(node);
	}

	public override void OnStart(StartState state)
	{
		mode = Mode.NoJoint;
		if (jointNodeName != string.Empty)
		{
			refNode = base.part.FindAttachNode(jointNodeName);
		}
		else
		{
			refNode = base.part.FindAttachNodeByPart(base.part.parent);
		}
		if (refNode != null)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				StartCoroutine(startAfterJointsCreated());
			}
		}
		else
		{
			Debug.LogError("[ModuleJointMotor]: Cannot initialize, no attachment node found with id " + jointNodeName, base.gameObject);
		}
		OnModuleStart(state);
	}

	public IEnumerator startAfterJointsCreated()
	{
		while (!base.part.started)
		{
			yield return null;
		}
		InitJoint();
	}

	public void InitJoint()
	{
		pJoint = findJointAtNode(refNode);
		if (pJoint != null)
		{
			joint = pJoint.Joint;
			axis = joint.axis;
			secAxis = joint.secondaryAxis;
			angXDrive = joint.angularXDrive;
			xAngle = 0f;
			lastXAngle = 0f;
			initOrt = getControlOrt(joint.secondaryAxis);
			dPos = Vector3.zero;
			dRot = Quaternion.identity;
			GameEvents.onPartJointBreak.Add(onPartJointBreak);
			mode = Mode.Park;
			OnJointInit(goodSetup: true);
		}
		else
		{
			OnJointInit(goodSetup: false);
		}
	}

	public PartJoint findJointAtNode(AttachNode node)
	{
		if (node.attachedPart != null)
		{
			if (node.attachedPart == base.part.parent)
			{
				return base.part.attachJoint;
			}
			return node.attachedPart.attachJoint;
		}
		return null;
	}

	public void onPartJointBreak(PartJoint pj, float force)
	{
		if (mode != 0 && pJoint == pj)
		{
			SetMotorMode(Mode.NoJoint);
			GameEvents.onPartJointBreak.Remove(onPartJointBreak);
		}
	}

	public void applyCoordsUpdate()
	{
		endOrt = getControlOrt(secAxis);
		xAngle = KSPUtil.HeadingDegrees(initOrt, endOrt, getControlOrt(axis));
		if (xAngle - lastXAngle != 0f)
		{
			joint.targetRotation = Quaternion.AngleAxis(xAngle, Vector3.left);
			dRot = Quaternion.AngleAxis(xAngle - lastXAngle, Quaternion.Inverse(pJoint.Host.vessel.transform.rotation) * pJoint.Host.partTransform.rotation * ((pJoint.Child == pJoint.Host) ? axis : (-axis)));
			lastXAngle = xAngle;
			xAngle = 0f;
			recurseCoordUpdate(pJoint.Child, dPos, dRot, Part.PartToVesselSpacePos(joint.anchor, pJoint.Host, pJoint.Host.vessel, PartSpaceMode.Current));
			dRot = Quaternion.identity;
			dPos.Zero();
		}
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

	public Vector3 getControlOrt(Vector3 refAxis)
	{
		return Quaternion.Inverse(pJoint.Target.partTransform.rotation) * pJoint.Host.partTransform.rotation * refAxis;
	}

	public void OnPartPack()
	{
		SetMotorMode(Mode.Park);
	}

	public void SetMotorSpeed(float motorSpeed)
	{
		if (mode == Mode.NoJoint)
		{
			Debug.LogError("[ModuleJointMotor]: Cannot set speed, no joint present.");
		}
		else
		{
			joint.targetAngularVelocity = Vector3.right * motorSpeed;
		}
	}

	public float GetMotorSpeed()
	{
		if (mode == Mode.NoJoint)
		{
			return 0f;
		}
		return joint.targetAngularVelocity.x;
	}

	public void SetMotorForce(float motorForce)
	{
		if (mode == Mode.NoJoint)
		{
			Debug.LogError("[ModuleJointMotor]: Cannot set force, no joint present.");
			return;
		}
		angXDrive.maximumForce = motorForce;
		joint.angularXDrive = angXDrive;
	}

	public float GetMotorForce()
	{
		if (mode == Mode.NoJoint)
		{
			return 0f;
		}
		return angXDrive.maximumForce;
	}

	public bool SetMotorMode(Mode m)
	{
		if (joint == null)
		{
			return false;
		}
		switch (m)
		{
		case Mode.Park:
			joint.angularXDrive = angXDrive;
			applyCoordsUpdate();
			break;
		case Mode.Neutral:
			joint.angularXDrive = angXDrive;
			break;
		case Mode.Drive:
			joint.angularXDrive = angXDrive;
			break;
		}
		if (m != mode)
		{
			mode = m;
			OnMotorModeChanged(mode);
		}
		base.vessel.CycleAllAutoStrut();
		return true;
	}

	public Mode GetMotorMode()
	{
		return mode;
	}

	public bool IsJointUnlocked()
	{
		if (joint != null)
		{
			return mode != Mode.Park;
		}
		return false;
	}

	public abstract void OnModuleSave(ConfigNode node);

	public abstract void OnModuleLoad(ConfigNode node);

	public abstract void OnModuleStart(StartState st);

	public abstract void OnJointInit(bool goodSetup);

	public abstract void OnMotorModeChanged(Mode mode);
}
