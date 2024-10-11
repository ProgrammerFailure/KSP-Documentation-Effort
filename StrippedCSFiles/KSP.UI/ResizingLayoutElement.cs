using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(LayoutElement))]
public class ResizingLayoutElement : MonoBehaviour
{
	private enum Direction
	{
		VERTICAL,
		HORIZONTAL,
		BOTH
	}

	[CompilerGenerated]
	private sealed class _003CResize_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ResizingLayoutElement _003C_003E4__this;

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
		public _003CResize_003Ed__22(int _003C_003E1__state)
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

	private LayoutElement layoutElement;

	private RectTransform rectTransform;

	private float startWidth;

	private float startHeight;

	private float endWidth;

	private float endHeight;

	private float duration;

	private bool destroyOnCompletion;

	private Callback onComplete;

	private Direction direction;

	private float currentHeight;

	private float currentWidth;

	private bool isReversing;

	private bool isResizing;

	public bool IsResizing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResizingLayoutElement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartResizing(float startHeight, float endHeight, float duration, bool destroyOnCompletion, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReverseResizing(bool destroyOnCompletion, Callback onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CResize_003Ed__22))]
	private IEnumerator Resize()
	{
		throw null;
	}
}
