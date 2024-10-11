using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mouse : MonoBehaviour
{
	public class MouseButton
	{
		[CompilerGenerated]
		private sealed class _003CTapRoutine_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MouseButton _003C_003E4__this;

			private float _003CendTime_003E5__2;

			private bool _003Ctapped_003E5__3;

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
			public _003CTapRoutine_003Ed__15(int _003C_003E1__state)
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

		private bool button;

		private bool down;

		private bool up;

		private bool tap;

		private bool doubleTap;

		private bool abort;

		private float doubleClickTime;

		private Vector2 dragDelta;

		private Vector2 pAtBtnDown;

		private Mouse owner;

		private int buttonIndex;

		public static RaycastHit hoveredPartHitInfo;

		private bool isTapStarted;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MouseButton(Mouse owner, int button)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003CTapRoutine_003Ed__15))]
		private IEnumerator TapRoutine()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ClearMouseState()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetButtonDown()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetButtonUp()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetButton()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetClick()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetDoubleClick(bool isDelegate = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector2 GetDragDelta()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool WasDragging(float delta = 400f)
		{
			throw null;
		}
	}

	[Flags]
	public enum Buttons
	{
		None = 0,
		Left = 1,
		Right = 2,
		Middle = 4,
		Btn4 = 8,
		Btn5 = 0x10,
		Any = -1
	}

	private static Mouse fetch;

	public static MouseButton Left;

	public static MouseButton Right;

	public static MouseButton Middle;

	public static Vector2 screenPos;

	private static Vector2 lastPos;

	public static Vector2 delta;

	public static PartPointer partPointer;

	public static Part HoveredPart;

	public static bool IsMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mouse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Mouse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Buttons GetAllMouseButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Buttons GetAllMouseButtonsUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Buttons GetAllMouseButtonsDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckButtons(Buttons buttons, Buttons buttonsToTest, bool strict = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Part CheckHoveredPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Part GetValidHoverPart(RaycastHit hitPart)
	{
		throw null;
	}
}
