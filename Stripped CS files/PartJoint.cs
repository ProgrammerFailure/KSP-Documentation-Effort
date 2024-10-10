using System.Collections.Generic;
using UnityEngine;

public class PartJoint : MonoBehaviour
{
	[SerializeField]
	public Part child;

	[SerializeField]
	public Part parent;

	[SerializeField]
	public Part host;

	[SerializeField]
	public Part target;

	[SerializeField]
	public Vector3 hostAnchor;

	[SerializeField]
	public Vector3 tgtAnchor;

	[SerializeField]
	public Vector3 axis;

	[SerializeField]
	public Vector3 secAxis;

	public float breakingForce;

	public float breakingTorque;

	public List<ConfigurableJoint> joints;

	public int internalJoints;

	public int i;

	public SoftJointLimit linearJointLimit;

	public SoftJointLimitSpring linearJointLimitSpring;

	public SoftJointLimit angularLimit;

	public SoftJointLimitSpring angularLimitSpring;

	public JointDrive linearDrive;

	public JointDrive angXDrive;

	public JointDrive angYZDrive;

	[SerializeField]
	public Vector3 tgtPos;

	[SerializeField]
	public Quaternion tgtRot;

	public AttachModes mode;

	public float rigidity = 1500f;

	public bool rigid;

	public float stackNodeFactor = 2f;

	public float srfNodeFactor = 0.8f;

	public float breakingForceModifier = 1f;

	public float breakingTorqueModifier = 1f;

	public bool goodSetup;

	public Part Child => child;

	public Part Parent => parent;

	public Part Host => host;

	public Part Target => target;

	public Vector3 HostAnchor => hostAnchor;

	public Vector3 TgtAnchor => tgtAnchor;

	public Vector3 Axis => axis;

	public Vector3 SecAxis => secAxis;

	public ConfigurableJoint Joint => joints[0];

	public float stiffness { get; set; }

	public static PartJoint Create(Part owner, Part parent, AttachNode nodeToParent, AttachNode nodeFromParent, AttachModes mode)
	{
		AttachNode attachNode = nodeToParent;
		Transform partTransform = owner.partTransform;
		bool flag = false;
		if (nodeToParent == null)
		{
			attachNode = nodeFromParent;
			partTransform = parent.partTransform;
			if (attachNode != null)
			{
				flag = attachNode.rigid;
			}
		}
		else if (nodeFromParent == null)
		{
			attachNode = nodeToParent;
			partTransform = owner.partTransform;
			flag = attachNode.rigid;
		}
		else
		{
			if (nodeToParent.size <= nodeFromParent.size)
			{
				attachNode = nodeToParent;
				partTransform = owner.partTransform;
			}
			else
			{
				attachNode = nodeFromParent;
				partTransform = parent.partTransform;
			}
			flag = nodeToParent.rigid || nodeFromParent.rigid;
		}
		if (attachNode == null)
		{
			attachNode = parent.srfAttachNode;
			partTransform = parent.partTransform;
			flag = attachNode.rigid;
		}
		return create(owner, parent, partTransform, attachNode.position, attachNode.orientation, attachNode.secondaryAxis, attachNode.size, mode, flag, nodeToParent, nodeFromParent);
	}

	public static PartJoint create(Part child, Part parent, Transform nodeSpace, Vector3 nodePos, Vector3 nodeOrt, Vector3 nodeOrt2, int nodeSize, AttachModes mode, bool rigid, AttachNode nodeToParent, AttachNode nodeFromParent)
	{
		Part part;
		if (child.physicalSignificance == Part.PhysicalSignificance.FULL && parent.physicalSignificance == Part.PhysicalSignificance.FULL)
		{
			part = child;
		}
		else if (child.physicalSignificance != 0)
		{
			part = parent;
		}
		else
		{
			if (parent.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				Debug.LogError("[PartJoint]: Cannot create a PartJoint between two physicsless parts. Something is very wrong here.", child);
				Debug.Break();
				return null;
			}
			part = child;
		}
		PartJoint partJoint = part.gameObject.AddComponent<PartJoint>();
		partJoint.child = child;
		partJoint.parent = parent;
		partJoint.host = part;
		partJoint.target = ((part == child) ? parent : child);
		nodePos = child.partTransform.InverseTransformPoint(nodeSpace.TransformPoint(nodePos));
		nodeOrt = child.partTransform.InverseTransformDirection(nodeSpace.TransformDirection(nodeOrt));
		nodeOrt2 = child.partTransform.InverseTransformDirection(nodeSpace.TransformDirection(nodeOrt2));
		partJoint.mode = mode;
		partJoint.rigid = rigid;
		partJoint.linearJointLimit = default(SoftJointLimit);
		partJoint.linearJointLimitSpring = default(SoftJointLimitSpring);
		partJoint.angularLimit = default(SoftJointLimit);
		partJoint.angularLimitSpring = default(SoftJointLimitSpring);
		partJoint.linearDrive = default(JointDrive);
		partJoint.angXDrive = default(JointDrive);
		partJoint.angYZDrive = default(JointDrive);
		if (nodeOrt2.IsZero())
		{
			Vector3.OrthoNormalize(ref nodeOrt, ref nodeOrt2);
		}
		partJoint.joints = new List<ConfigurableJoint>();
		if (nodeSize >= 2 && !child.HasModuleImplementing<ModuleDockingNode>() && !parent.HasModuleImplementing<ModuleDockingNode>())
		{
			partJoint.internalJoints = 3;
			for (int i = 0; i < partJoint.internalJoints; i++)
			{
				ConfigurableJoint configurableJoint = partJoint.SetupJoint(nodePos + Quaternion.AngleAxis(120f * (float)i, nodeOrt) * (nodeOrt2 * ((float)nodeSize - 1f)), nodeOrt, nodeOrt2, nodeSize, nodeToParent, nodeFromParent);
				if (configurableJoint != null)
				{
					partJoint.joints.Add(configurableJoint);
				}
				else
				{
					partJoint.internalJoints--;
				}
			}
		}
		else
		{
			partJoint.internalJoints = 1;
			ConfigurableJoint configurableJoint2 = partJoint.SetupJoint(nodePos, nodeOrt, nodeOrt2, nodeSize, nodeToParent, nodeFromParent);
			if (configurableJoint2 != null)
			{
				partJoint.joints.Add(configurableJoint2);
			}
			else
			{
				partJoint.internalJoints = 0;
			}
		}
		partJoint.goodSetup = true;
		return partJoint;
	}

	public ConfigurableJoint SetupJoint(Vector3 jointPos, Vector3 jointOrt, Vector3 jointOrt2, int size, AttachNode nodeToParent, AttachNode nodeFromParent)
	{
		GameObject gameObject = host.gameObject;
		Transform partTransform = child.partTransform;
		target.isRobotic(out var servo);
		child.isRobotic(out var servo2);
		Rigidbody rigidbody = target.Rigidbody;
		if (servo != null || servo2 != null)
		{
			if (servo2 != null)
			{
				if (nodeToParent != null && nodeToParent.nodeType == AttachNode.NodeType.Surface)
				{
					AttachNode node = null;
					float num = float.MaxValue;
					for (int i = 0; i < child.attachNodes.Count; i++)
					{
						float num2 = Vector3.Distance(nodeToParent.position, child.attachNodes[i].position);
						if (num2 < num)
						{
							node = child.attachNodes[i];
							num = num2;
						}
					}
					gameObject = servo2.NodeRigidBody(node).gameObject;
					partTransform = gameObject.transform;
				}
				else
				{
					rigidbody = servo2.AttachServoRigidBody((nodeToParent == null) ? nodeFromParent : nodeToParent);
					if (rigidbody != target.Rigidbody)
					{
						gameObject = rigidbody.gameObject;
						partTransform = servo2.MovingObject().transform;
						rigidbody = target.Rigidbody;
					}
				}
			}
			if (servo != null)
			{
				rigidbody = servo.AttachServoRigidBody((nodeFromParent == null) ? nodeToParent : nodeFromParent);
				if (host.Rigidbody != null && rigidbody == host.Rigidbody)
				{
					rigidbody = target.Rigidbody;
				}
			}
		}
		ConfigurableJoint configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
		if (rigidbody != host.Rigidbody)
		{
			configurableJoint.connectedBody = rigidbody;
		}
		else if (target.parent != null && target.parent.Rigidbody != null && target.parent.Rigidbody != rigidbody)
		{
			configurableJoint.connectedBody = target.parent.Rigidbody;
		}
		else
		{
			if (!(target.parent != null) || !(target.parent.parent != null) || !(target.parent.parent.Rigidbody != rigidbody))
			{
				return null;
			}
			configurableJoint.connectedBody = target.parent.parent.Rigidbody;
		}
		hostAnchor = gameObject.transform.InverseTransformPoint(partTransform.TransformPoint(jointPos));
		if (configurableJoint.connectedBody == null)
		{
			return null;
		}
		tgtAnchor = configurableJoint.connectedBody.transform.InverseTransformPoint(partTransform.TransformPoint(jointPos));
		axis = gameObject.transform.InverseTransformDirection(partTransform.TransformDirection(jointOrt));
		secAxis = gameObject.transform.InverseTransformDirection(partTransform.TransformDirection(jointOrt2));
		configurableJoint.autoConfigureConnectedAnchor = false;
		configurableJoint.anchor = hostAnchor;
		configurableJoint.axis = axis;
		configurableJoint.secondaryAxis = secAxis;
		configurableJoint.connectedAnchor = tgtAnchor;
		configurableJoint.targetVelocity = Vector3.zero;
		float num3 = 0f;
		if (mode == AttachModes.STACK)
		{
			num3 = ((float)size + 1f) * stackNodeFactor;
			stiffness = rigidity * num3 * FlightGlobals.StackAttachStiffNess;
		}
		else if (mode == AttachModes.SRF_ATTACH)
		{
			num3 = ((float)size + 1f) * srfNodeFactor;
			stiffness = rigidity * num3 * FlightGlobals.SrfAttachStiffNess;
		}
		SetJointLimits(configurableJoint, rigid);
		configurableJoint.targetRotation = Quaternion.identity;
		configurableJoint.targetAngularVelocity = Vector3.zero;
		breakingForce = Mathf.Min(host.breakingForce, target.breakingForce) * breakingForceModifier * num3 / (float)internalJoints;
		breakingTorque = Mathf.Min(host.breakingTorque, target.breakingTorque) * breakingTorqueModifier * num3 / (float)internalJoints;
		configurableJoint.breakForce = float.PositiveInfinity;
		configurableJoint.breakTorque = float.PositiveInfinity;
		configurableJoint.configuredInWorldSpace = false;
		return configurableJoint;
	}

	public void SetUnbreakable(bool unbreakable, bool forceRigid)
	{
		if (!goodSetup)
		{
			return;
		}
		bool flag = rigid || forceRigid;
		if (unbreakable)
		{
			for (i = 0; i < internalJoints; i++)
			{
				if (i <= joints.Count - 1 && joints[i] != null)
				{
					joints[i].breakForce = float.PositiveInfinity;
					joints[i].breakTorque = float.PositiveInfinity;
					SetJointLimits(joints[i], flag);
				}
			}
		}
		else
		{
			for (i = 0; i < internalJoints; i++)
			{
				if (i <= joints.Count - 1 && joints[i] != null)
				{
					joints[i].breakForce = breakingForce * PhysicsGlobals.JointBreakForceFactor * (flag ? PhysicsGlobals.RigidJointBreakForceFactor : 1f);
					joints[i].breakTorque = breakingTorque * PhysicsGlobals.JointBreakTorqueFactor * (flag ? PhysicsGlobals.RigidJointBreakTorqueFactor : 1f);
					SetJointLimits(joints[i], flag);
				}
			}
		}
		GameEvents.onPartJointSet.Fire(this);
	}

	public void SetBreakingForces(float breakForce, float breakTorque)
	{
		if (goodSetup)
		{
			breakingForce = breakForce;
			breakingTorque = breakTorque;
			SetUnbreakable(Joint.breakForce == float.PositiveInfinity, rigid);
		}
	}

	public void SetJointLimits(ConfigurableJoint newJoint, bool rigidAttach)
	{
		ConfigurableJointMotion zMotion = (newJoint.yMotion = (newJoint.xMotion = (newJoint.angularZMotion = (newJoint.angularYMotion = (newJoint.angularXMotion = ((!rigidAttach) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked))))));
		newJoint.zMotion = zMotion;
		angularLimit.limit = (rigidAttach ? 0f : 180f);
		angularLimit.bounciness = 0f;
		angularLimitSpring.spring = 0f;
		angularLimitSpring.damper = 0f;
		linearJointLimit.limit = 1f;
		linearJointLimit.bounciness = 0f;
		linearJointLimitSpring.damper = 0f;
		linearJointLimitSpring.spring = 0f;
		linearDrive.maximumForce = PhysicsGlobals.JointForce;
		linearDrive.positionDamper = 0f;
		linearDrive.positionSpring = PhysicsGlobals.JointForce;
		newJoint.xDrive = linearDrive;
		newJoint.yDrive = linearDrive;
		newJoint.zDrive = linearDrive;
		newJoint.targetPosition = Vector3.zero;
		angXDrive.maximumForce = (rigidAttach ? float.PositiveInfinity : PhysicsGlobals.JointForce);
		angXDrive.positionDamper = 0f;
		angXDrive.positionSpring = (rigidAttach ? float.PositiveInfinity : stiffness);
		angYZDrive.maximumForce = (rigidAttach ? float.PositiveInfinity : PhysicsGlobals.JointForce);
		angYZDrive.positionDamper = 0f;
		angYZDrive.positionSpring = (rigidAttach ? float.PositiveInfinity : stiffness);
		newJoint.rotationDriveMode = RotationDriveMode.XYAndZ;
		newJoint.angularXDrive = angXDrive;
		newJoint.angularYZDrive = angYZDrive;
		newJoint.highAngularXLimit = angularLimit;
		newJoint.lowAngularXLimit = angularLimit;
		newJoint.angularYLimit = angularLimit;
		newJoint.angularZLimit = angularLimit;
		newJoint.angularXLimitSpring = angularLimitSpring;
		newJoint.angularYZLimitSpring = angularLimitSpring;
		newJoint.linearLimit = linearJointLimit;
		newJoint.linearLimitSpring = linearJointLimitSpring;
	}

	public void OnJointBreak(float breakForce)
	{
		if (host.vessel != null && host.vessel.IgnoreCollisionsFrames > 0)
		{
			return;
		}
		if (GameSettings.LOG_JOINT_BREAK_EVENT)
		{
			Debug.Log("[PartJoint]: " + base.name + " joint broke under a force of " + breakForce.ToString("F2"), base.gameObject);
		}
		GameEvents.onPartJointBreak.Fire(this, breakForce);
		child.OnPartJointBreak(breakForce);
		for (i = 0; i < internalJoints; i++)
		{
			if (joints[i] != null)
			{
				Object.Destroy(joints[i]);
			}
		}
		Object.Destroy(this);
	}

	public void OnPartPack()
	{
	}

	public void OnPartUnpack()
	{
	}

	public void DestroyJoint()
	{
		GameEvents.onPartJointBreak.Fire(this, 0f);
		for (i = 0; i < internalJoints; i++)
		{
			Object.Destroy(joints[i]);
		}
		Object.Destroy(this);
	}

	public void OnDestroy()
	{
	}

	public Vector3 JointToLocalSpacePos(Vector3 pos)
	{
		return Quaternion.FromToRotation(axis, Vector3.right) * Quaternion.FromToRotation(Vector3.up, secAxis) * (pos + hostAnchor);
	}

	public Vector3 JointToLocalSpaceDir(Vector3 dir)
	{
		return Quaternion.FromToRotation(Vector3.right, axis) * Quaternion.FromToRotation(Vector3.up, secAxis) * dir;
	}

	public Quaternion JointToLocalSpaceRot(Quaternion rot)
	{
		return Quaternion.FromToRotation(axis, Vector3.right) * Quaternion.FromToRotation(Vector3.up, secAxis) * rot;
	}

	public Vector3 LocaltoJointSpacePos(Vector3 pos)
	{
		return Quaternion.FromToRotation(secAxis, Vector3.up) * (Quaternion.FromToRotation(axis, Vector3.right) * (pos - hostAnchor));
	}

	public Vector3 LocaltoJointSpaceDir(Vector3 dir)
	{
		return Quaternion.FromToRotation(secAxis, Vector3.up) * (Quaternion.FromToRotation(axis, Vector3.right) * dir);
	}

	public Quaternion LocaltoJointSpaceRot(Quaternion rot)
	{
		return Quaternion.FromToRotation(secAxis, Vector3.up) * (Quaternion.FromToRotation(axis, Vector3.right) * rot);
	}

	public static Vector3 JointToLocalSpacePos(Vector3 pos, PartJoint joint)
	{
		return joint.JointToLocalSpacePos(pos);
	}

	public static Vector3 JointToLocalSpaceDir(Vector3 dir, PartJoint joint)
	{
		return joint.JointToLocalSpaceDir(dir);
	}

	public static Quaternion JointToLocalSpaceRot(Quaternion rot, PartJoint joint)
	{
		return joint.JointToLocalSpaceRot(rot);
	}

	public static Vector3 LocaltoJointSpacePos(Vector3 pos, PartJoint joint)
	{
		return joint.LocaltoJointSpacePos(pos);
	}

	public static Vector3 LocaltoJointSpaceDir(Vector3 dir, PartJoint joint)
	{
		return joint.LocaltoJointSpaceDir(dir);
	}

	public static Quaternion LocaltoJointSpaceRot(Quaternion rot, PartJoint joint)
	{
		return joint.LocaltoJointSpaceRot(rot);
	}
}
