using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CameraFXModules;

public class Fade : CameraFXModule
{
	[CompilerGenerated]
	private sealed class _003CRemoveSelf_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Fade _003C_003E4__this;

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
		public _003CRemoveSelf_003Ed__14(int _003C_003E1__state)
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

	private CameraFXModule fxMod;

	private CameraFXCollection host;

	public float duration;

	public float falloff;

	public float fxScale;

	public float T0;

	public float T;

	private bool willRemove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Fade(CameraFXModule fxModule, float duration, float falloff = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnFXAdded(CameraFXCollection host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnFXRemoved(CameraFXCollection host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Vector3 UpdateLocalPosition(Vector3 defaultPos, Vector3 currPos, float m, Views viewMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Quaternion UpdateLocalRotation(Quaternion defaultRot, Quaternion currRot, float m, Views viewMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFade(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRemoveSelf_003Ed__14))]
	private IEnumerator RemoveSelf()
	{
		throw null;
	}
}
