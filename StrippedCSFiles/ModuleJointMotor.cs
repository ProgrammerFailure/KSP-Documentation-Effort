using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

	[CompilerGenerated]
	private sealed class _003CstartAfterJointsCreated_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleJointMotor _003C_003E4__this;

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
		public _003CstartAfterJointsCreated_003Ed__27(int _003C_003E1__state)
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

	[KSPField]
	public string jointNodeName;

	private Vector3 dPos;

	private Quaternion dRot;

	private Vector3 axis;

	private Vector3 secAxis;

	private JointDrive angXDrive;

	private Vector3 initOrt;

	private Vector3 endOrt;

	private Vector3 ctrlAxis;

	private float xAngle;

	private float lastXAngle;

	public Mode mode;

	protected AttachNode refNode
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

	protected PartJoint pJoint
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

	protected ConfigurableJoint joint
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
	protected ModuleJointMotor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CstartAfterJointsCreated_003Ed__27))]
	private IEnumerator startAfterJointsCreated()
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
	private void onPartJointBreak(PartJoint pj, float force)
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
	private Vector3 getControlOrt(Vector3 refAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMotorSpeed(float motorSpeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetMotorSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMotorForce(float motorForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetMotorForce()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool SetMotorMode(Mode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Mode GetMotorMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsJointUnlocked()
	{
		throw null;
	}

	protected abstract void OnModuleSave(ConfigNode node);

	protected abstract void OnModuleLoad(ConfigNode node);

	protected abstract void OnModuleStart(StartState st);

	protected abstract void OnJointInit(bool goodSetup);

	protected abstract void OnMotorModeChanged(Mode mode);
}
