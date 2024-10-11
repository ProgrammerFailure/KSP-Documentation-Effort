using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CreditsMultiTextHeader : MonoBehaviour
{
	[Serializable]
	public class TextHeader
	{
		public Transform headerRef;

		public Transform currentAnchor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextHeader()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CshiftText_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float duration;

		public TextHeader textTrf;

		public Transform tgtAnchor;

		private float _003Ct_003E5__2;

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
		public _003CshiftText_003Ed__10(int _003C_003E1__state)
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

	public TextHeader[] textTrfs;

	private int currentText;

	public Transform startAnchor;

	public Transform centerAnchor;

	public Transform endAnchor;

	public float transitionDuration;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CreditsMultiTextHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(int idx, float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NextText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CshiftText_003Ed__10))]
	private IEnumerator shiftText(TextHeader textTrf, Transform tgtAnchor, float duration)
	{
		throw null;
	}
}
