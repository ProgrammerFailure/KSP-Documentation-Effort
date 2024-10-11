using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KerbalExpressionAI : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CkerbalAvatarUpdateCycle_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalExpressionAI _003C_003E4__this;

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
		public _003CkerbalAvatarUpdateCycle_003Ed__29(int _003C_003E1__state)
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

	public ProtoCrewMember protoCrewMember;

	public Mesh idleMesh;

	public Mesh[] wheeeMeshes;

	public Mesh[] panicMeshes;

	public Transform headRef;

	public Transform neckJoint;

	public MeshFilter headMesh;

	public float panicLevel;

	public float wheeeLevel;

	public float idleThreshold;

	protected Mesh currentMesh;

	public float headBobAmount;

	public float updateInterval;

	public float noiseSeed;

	protected Quaternion helmetInitRot;

	public float flight_velocity;

	public float flight_angularV;

	public float flight_gee;

	public float fearFactor;

	protected float expression;

	protected bool running;

	protected bool isBadass;

	protected float courage;

	protected float stupidity;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalExpressionAI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void explosionReaction(GameEvents.ExplosionReaction er)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CkerbalAvatarUpdateCycle_003Ed__29))]
	protected virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateExpressionAI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateHeadMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateHeadTransforms()
	{
		throw null;
	}
}
