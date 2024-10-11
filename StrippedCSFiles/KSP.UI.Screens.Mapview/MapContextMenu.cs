using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview.MapContextMenuOptions;
using UnityEngine;

namespace KSP.UI.Screens.Mapview;

public class MapContextMenu
{
	[CompilerGenerated]
	private sealed class _003CdialogUpdateCoroutine_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MapContextMenu _003C_003E4__this;

		private bool _003CzPos_003E5__2;

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
		public _003CdialogUpdateCoroutine_003Ed__19(int _003C_003E1__state)
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

	private string header;

	private PopupDialog dialog;

	private MapContextMenuOption[] options;

	internal readonly IScreenCaster sc;

	private WaitForEndOfFrame updateYield;

	private RectTransform dialogTrf;

	private Callback onDismiss;

	public string Header
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapContextMenu(string header, IScreenCaster sc, Callback onDismiss, MapContextMenuOption[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapContextMenu(string header, Rect rct, IScreenCaster sc, Callback onDismiss, MapContextMenuOption[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapContextMenu Create(string header, IScreenCaster sc, Callback onDismiss, params MapContextMenuOption[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapContextMenu Create(string header, Rect rct, IScreenCaster sc, Callback onDismiss, params MapContextMenuOption[] options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onDialogDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CdialogUpdateCoroutine_003Ed__19))]
	private IEnumerator dialogUpdateCoroutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DialogUpdate(bool zPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdatePosition()
	{
		throw null;
	}
}
