using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorSubassemblyItem : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003COnDismissCoroutine_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003COnDismissCoroutine_003Ed__14(int _003C_003E1__state)
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

	public TextMeshProUGUI title;

	public TextMeshProUGUI description;

	public TextMeshProUGUI partcount;

	public Button btnSpawnSubassembly;

	public Button btnDelete;

	private EditorPartList partList;

	private ShipTemplate subassembly;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorSubassemblyItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(EditorPartList partList, ShipTemplate sa)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_Spawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_Delete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteSubassembly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003COnDismissCoroutine_003Ed__14))]
	private IEnumerator OnDismissCoroutine()
	{
		throw null;
	}
}
