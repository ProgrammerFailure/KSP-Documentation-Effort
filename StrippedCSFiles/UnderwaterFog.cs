using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnderwaterFog : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UnderwaterFog _003C_003E4__this;

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
		public _003CStart_003Ed__24(int _003C_003E1__state)
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

	private bool oldFog;

	private FogMode oldFogMode;

	private float oldFogDensity;

	private Color oldFogColor;

	private float oldSunBrightnessMult;

	private float oldSunFlareBrightnessMult;

	public float alt;

	public Vector4 camVectorPos;

	public Vector4 camVectorDir;

	public float lastAlt;

	public bool isScaled;

	public bool foundAfg;

	public bool fxOn;

	public bool isPQS;

	protected int ShaderGlobalCamVectorPos;

	protected int ShaderGlobalCamVectorDir;

	protected int ShaderGlobal_UnderwaterFogColor;

	protected int ShaderGlobal_UnderwaterMinAlphaFogDistance;

	protected int ShaderGlobal_UnderwaterMaxAlbedoFog;

	protected int ShaderGlobal_UnderwaterMaxAlphaFog;

	protected int ShaderGlobal_UnderwaterAlbedoDistanceScalar;

	protected int ShaderGlobal_UnderwaterAlphaDistanceScalar;

	protected CelestialBody curBody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UnderwaterFog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__24))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneChange(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDefaultCB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateShaderPropertiesDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateShaderProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCBProperties(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPreRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPostRender()
	{
		throw null;
	}
}
