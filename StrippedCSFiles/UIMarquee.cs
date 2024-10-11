using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMarquee : MonoBehaviour
{
	public enum ScrollType
	{
		Left,
		Right,
		LeftThenRight,
		RightThenLeft
	}

	[CompilerGenerated]
	private sealed class _003CInitialize_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIMarquee _003C_003E4__this;

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
		public _003CInitialize_003Ed__34(int _003C_003E1__state)
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
	private sealed class _003CMoveMarquee_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIMarquee _003C_003E4__this;

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
		public _003CMoveMarquee_003Ed__44(int _003C_003E1__state)
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

	[SerializeField]
	protected TextMeshProUGUI targetLabel;

	protected Transform labelParentGO;

	protected GameObject marqueeObject;

	protected float marqueeWidth;

	protected ContentSizeFitter labelFitter;

	protected RectMask2D mask;

	protected Vector3 movingTransform;

	protected float leftX;

	protected float rightX;

	protected float leftXBound;

	protected float rightXBound;

	protected PointerEnterExitHandler enterExitHandler;

	protected bool movingLeft;

	public ScrollType scrollMode;

	public bool loop;

	public float delayAfterLoop;

	public float delayBeforeLoop;

	public bool autoScrollOnShow;

	public bool stopOnMouseOff;

	public float startDelay;

	public float movingOverrun;

	public bool isMoving;

	public TextOverflowModes staticOverflowMode;

	public TextOverflowModes movingOverflowMode;

	public TextAlignmentOptions staticHorizontalAlign;

	public TextAlignmentOptions movingHorizontalAlign;

	protected bool ready;

	protected bool isActive;

	protected bool isMouseOver;

	protected bool isRightAnchored;

	protected IEnumerator marqueeMovementCoroutine;

	protected float originalFontSizeMax;

	public float movingSpeed;

	protected float movementAmount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIMarquee()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CInitialize_003Ed__34))]
	protected IEnumerator Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Field_OnValueModified(object arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reconfigure Marquee")]
	internal void Configure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void StartMarquee()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void StopMarquee()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AutoStartMarquee()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CMoveMarquee_003Ed__44))]
	private IEnumerator MoveMarquee()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetMovingFlags(bool comingFromRight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
