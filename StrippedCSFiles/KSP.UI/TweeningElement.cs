using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class TweeningElement : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTween_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TweeningElement _003C_003E4__this;

		private float _003CtimeAtLastFrame_003E5__2;

		private float _003Ctime_003E5__3;

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
		public _003CTween_003Ed__13(int _003C_003E1__state)
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

	[HideInInspector]
	public object data;

	[HideInInspector]
	public int dataInt;

	private RectTransform tweeningElement;

	private Vector3 startPos;

	private Vector3 endPos;

	private RectTransform endPosByElement;

	private bool endAtElement;

	private float duration;

	private TweeningController.TweeningFunction tweeningFunction;

	private Callback onComplete;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TweeningElement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartTweening(RectTransform tweeningElement, Vector3 startPos, Vector3 endPos, float duration, TweeningController.TweeningFunction tweeningFunction, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartTweening(RectTransform tweeningElement, Vector3 startPos, RectTransform endPosByElement, float duration, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CTween_003Ed__13))]
	private IEnumerator Tween()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float EaseInBack(float value)
	{
		throw null;
	}
}
