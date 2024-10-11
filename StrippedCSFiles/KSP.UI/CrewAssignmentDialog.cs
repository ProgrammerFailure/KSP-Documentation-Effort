using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KSP.UI;

public class CrewAssignmentDialog : BaseCrewAssignmentDialog
{
	[CompilerGenerated]
	private sealed class _003CRefreshCrewOnDrop_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrewAssignmentDialog _003C_003E4__this;

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
		public _003CRefreshCrewOnDrop_003Ed__13(int _003C_003E1__state)
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

	public static CrewAssignmentDialog Instance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewAssignmentDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override KerbalRoster GetCurrentCrewRoster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonAstronautComplex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOpenAstronautComplex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOpenACProceed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOpenACDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRefreshCrewOnDrop_003Ed__13))]
	private IEnumerator RefreshCrewOnDrop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditorLogicUpdateCrew()
	{
		throw null;
	}
}
