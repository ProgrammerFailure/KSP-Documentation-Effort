using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class kerbalExpressionSystem : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CSetNewIdleExpression_003Ed__69 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public kerbalExpressionSystem _003C_003E4__this;

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
		public _003CSetNewIdleExpression_003Ed__69(int _003C_003E1__state)
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

	public Part evaPart;

	public Animator animator;

	public string expressionParameterName;

	public string varianceParameterName;

	public string secondaryVarianceParameterName;

	public string hasHelmetName;

	public string idleBoolName;

	public string idleFloatName;

	public Kerbal kerbal;

	public KerbalEVA kerbalEVA;

	public float panicLevel;

	public float wheeLevel;

	public float fearFactor;

	public float expression;

	public float expressionLerpRate;

	public float idleThreshold;

	public int idleRandomNumberLimitHelmet;

	public int idleRandomNumberLimitNoHelmet;

	public float varianceLerpRate;

	public float varianceUpdateInterval;

	public float varianceTimer;

	public float secondaryVarianceLerpRate;

	public float secondaryVarianceUpdateInterval;

	public float secondaryVarianceTimer;

	public float idleTimer;

	public float idleUpdateInterval;

	public int idlePlayChance;

	public float flight_velocity;

	public float flight_angularV;

	public float flight_gee;

	protected int expressionSampleSize;

	protected int currentExpressionCacheIndex;

	protected float[] expressionSampleCache;

	public bool isIVAController;

	protected bool isBadass;

	protected float IdleFloat;

	protected bool isIdle;

	protected bool hasHelmet;

	protected float courage;

	protected float stupidity;

	protected float variance;

	protected float secondaryVariance;

	protected float targetVariance;

	protected float targetSecondaryVariance;

	protected float targetExpression;

	protected float normalizedExpression;

	protected int expressionHash;

	protected int varianceHash;

	protected int secondaryVarianceHash;

	protected int idleBoolHash;

	protected int idleFloatHash;

	protected int hasHelmetHash;

	protected bool running;

	protected bool isJetpackOn;

	public float newIdleWheeLevel;

	public float newIdleFearFactor;

	public float startExpressionTime;

	public bool useNewIdleExpressions;

	public static bool showInactive;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public kerbalExpressionSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static kerbalExpressionSystem()
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
	protected virtual void IVAUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void EVAUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetNewIdleExpression_003Ed__69))]
	public IEnumerator SetNewIdleExpression()
	{
		throw null;
	}
}
