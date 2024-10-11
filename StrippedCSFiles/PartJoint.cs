using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartJoint : MonoBehaviour
{
	[SerializeField]
	private Part child;

	[SerializeField]
	private Part parent;

	[SerializeField]
	private Part host;

	[SerializeField]
	private Part target;

	[SerializeField]
	private Vector3 hostAnchor;

	[SerializeField]
	private Vector3 tgtAnchor;

	[SerializeField]
	private Vector3 axis;

	[SerializeField]
	private Vector3 secAxis;

	private float breakingForce;

	private float breakingTorque;

	public List<ConfigurableJoint> joints;

	private int internalJoints;

	private int i;

	private SoftJointLimit linearJointLimit;

	private SoftJointLimitSpring linearJointLimitSpring;

	private SoftJointLimit angularLimit;

	private SoftJointLimitSpring angularLimitSpring;

	private JointDrive linearDrive;

	private JointDrive angXDrive;

	private JointDrive angYZDrive;

	[SerializeField]
	private Vector3 tgtPos;

	[SerializeField]
	private Quaternion tgtRot;

	private AttachModes mode;

	private float rigidity;

	private bool rigid;

	private float stackNodeFactor;

	private float srfNodeFactor;

	private float breakingForceModifier;

	private float breakingTorqueModifier;

	private bool goodSetup;

	public Part Child
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Parent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Host
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Target
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 HostAnchor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 TgtAnchor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 Axis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 SecAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigurableJoint Joint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float stiffness
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartJoint Create(Part owner, Part parent, AttachNode nodeToParent, AttachNode nodeFromParent, AttachModes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static PartJoint create(Part child, Part parent, Transform nodeSpace, Vector3 nodePos, Vector3 nodeOrt, Vector3 nodeOrt2, int nodeSize, AttachModes mode, bool rigid, AttachNode nodeToParent, AttachNode nodeFromParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigurableJoint SetupJoint(Vector3 jointPos, Vector3 jointOrt, Vector3 jointOrt2, int size, AttachNode nodeToParent, AttachNode nodeFromParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUnbreakable(bool unbreakable, bool forceRigid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBreakingForces(float breakForce, float breakTorque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetJointLimits(ConfigurableJoint newJoint, bool rigidAttach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnJointBreak(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 JointToLocalSpacePos(Vector3 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 JointToLocalSpaceDir(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion JointToLocalSpaceRot(Quaternion rot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 LocaltoJointSpacePos(Vector3 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 LocaltoJointSpaceDir(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion LocaltoJointSpaceRot(Quaternion rot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 JointToLocalSpacePos(Vector3 pos, PartJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 JointToLocalSpaceDir(Vector3 dir, PartJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion JointToLocalSpaceRot(Quaternion rot, PartJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 LocaltoJointSpacePos(Vector3 pos, PartJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 LocaltoJointSpaceDir(Vector3 dir, PartJoint joint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion LocaltoJointSpaceRot(Quaternion rot, PartJoint joint)
	{
		throw null;
	}
}
