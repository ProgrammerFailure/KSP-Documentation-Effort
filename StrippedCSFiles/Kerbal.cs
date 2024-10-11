using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight;
using UnityEngine;

public class Kerbal : MonoBehaviour
{
	public enum States
	{
		BAILED_OUT,
		DEAD,
		NO_SIGNAL,
		ALIVE
	}

	[CompilerGenerated]
	private sealed class _003CkerbalAvatarUpdateCycle_003Ed__50 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Kerbal _003C_003E4__this;

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
		public _003CkerbalAvatarUpdateCycle_003Ed__50(int _003C_003E1__state)
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

	public string crewMemberName;

	public float courage;

	public float stupidity;

	public bool isBadass;

	public bool veteran;

	public ProtoCrewMember.RosterStatus rosterStatus;

	public Part InPart;

	public ProtoCrewMember protoCrewMember;

	public float noiseSeed;

	public float camBobAmount;

	public Renderer[] textureTargets;

	public Texture2D textureStandard;

	public Texture2D textureVeteran;

	public Transform helmetTransform;

	public Transform headTransform;

	public States state;

	public float staticOverlayDuration;

	public float updateInterval;

	protected bool running;

	protected WaitForSeconds updIntervalYield;

	protected Coroutine updateCoroutine;

	protected bool EVAisUnlocked;

	public Camera kerbalCam;

	public Transform camPivot;

	protected KerbalPortrait portrait;

	public RenderTexture avatarTexture;

	protected bool visibleInPortrait;

	[HideInInspector]
	public bool showHelmet;

	protected bool ivaMode;

	protected IVACamera ivaCamera;

	public Transform eyeTransform;

	public bool randomizeOnStartup;

	public Vector3 eyeInitialPos;

	public bool capturedInitial;

	protected Renderer[] headRenderers;

	protected Animator[] animators;

	private FieldInfo canvasWillRenderCanvasesFieldInfo;

	public Vector2 screenPos;

	public Vessel InVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Renderer[] HeadRenderers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Animator[] Animators
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Kerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyAvatarTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupKerbalModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ShowHelmet(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CkerbalAvatarUpdateCycle_003Ed__50))]
	protected virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void kerbalAvatarUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void kerbalSeatCamUpdate(Camera seatCamera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CameraOverlayUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckEVAUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void IVAEnable(bool setIVAMode = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void IVADisable(bool setIVAMode = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetVisibleInPortrait(bool visible)
	{
		throw null;
	}
}
