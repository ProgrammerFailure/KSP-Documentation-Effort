using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

	private struct Coords
	{
		public Vector3 position;

		public Quaternion rotation;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Coords(Vector3 pos, Quaternion rot)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CstartAfterJointsCreated_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActiveJoint _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CstartAfterJointsCreated_003Ed__47(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CwaitAndRestart_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActiveJoint _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CwaitAndRestart_003Ed__49(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	protected AttachNode refNode;

	protected bool targetParent;

	protected Vector3 dPos;

	protected Vector3 lastDPos;

	protected Quaternion dRot;

	protected Vector3 axis;

	protected Vector3 secAxis;

	protected Vector3 anchor;

	protected Vector3 pivot;

	protected Vector3 lastPivot;

	protected Vector3 ctrlAxis;

	protected Vector3 activeAxis;

	protected Vector3 initOrt;

	protected Vector3 lastOrt;

	protected Vector3 endOrt;

	protected float Angle;

	protected float lastAngle;

	protected Quaternion jointRot0;

	protected Quaternion lastDRot;

	public float maxJointDamper;

	public JointDrive targetDrive;

	protected IActiveJointHost moduleHost;

	protected Part hostPart;

	private Vector3 dInitOrt;

	private Vector3 dEndOrt;

	private Vector3 dCrossVector;

	private Vector3 jointSpaceAxis;

	public PartJoint pJoint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public ConfigurableJoint joint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public JointMode jointMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public DriveMode driveMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public bool isValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActiveJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ActiveJoint Create(IActiveJointHost moduleHost, string refNodeId, JointMode function)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ActiveJoint Create(IActiveJointHost moduleHost, AttachNode refNode, JointMode function)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void startForRefNode(string jointNodeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CstartAfterJointsCreated_003Ed__47))]
	private IEnumerator startAfterJointsCreated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void restartJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndRestart_003Ed__49))]
	private IEnumerator waitAndRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void InitJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartJoint findJointAtNode(AttachNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartJoint findJointBetweenParts(Part p1, Part p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartJoint findAttachJointForTargetPart(Part p, Rigidbody connectedBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected JointDrive GetJointDrive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetJointDrive(JointDrive drive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetDriveMode(DriveMode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartDestroyed(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartJointBreak(PartJoint pj, float force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartPack(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onActiveJointNeedUpdate(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 GetCtrlAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getControlOrt(Vector3 refAxis, PartSpaceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getInvControlOrt(Vector3 refAxis, PartSpaceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getControlPos(Vector3 refPos, PartSpaceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getInvControlPos(Vector3 refPos, PartSpaceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void applyCoordsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void recurseCoordUpdate(Part p, Vector3 dPos, Quaternion dRot, Vector3 pivot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAxis(Transform srcSpace, Vector3 axis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSecondaryAxis(Transform srcSpace, Vector3 secondAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getAnchorOffset(PartSpaceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onJointInit(bool jointExists)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawDebug()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Coords> storeVesselCoords()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void restoreVesselCoords(List<Coords> storedCoords)
	{
		throw null;
	}
}
