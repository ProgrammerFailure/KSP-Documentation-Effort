using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class RDTechTreeSearchBar : MonoBehaviour
{
	protected enum MatchType
	{
		NONE,
		EQUALS_ONLY,
		TERM_STARTS_WITH_TAG,
		TERM_ENDS_WITH_TAG,
		TAG_STARTS_WITH_TERM,
		TAG_ENDS_WITH_TERM,
		EITHER_STARTS_WITH_EITHER,
		EITHER_ENDS_WITH_EITHER,
		TERM_CONTAINS_TAG,
		TAG_CONTAINS_TERM,
		EITHER_CONTAINS_EITHER
	}

	[CompilerGenerated]
	private sealed class _003CSearchRoutine_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RDTechTreeSearchBar _003C_003E4__this;

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
		public _003CSearchRoutine_003Ed__16(int _003C_003E1__state)
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

	public static RDTechTreeSearchBar Instance;

	protected float searchTimer;

	protected Coroutine searchRoutine;

	public float searchKeystrokeDelay;

	protected Image searchFieldBackground;

	protected PointerClickHandler searchFieldClickHandler;

	private List<RDNode> treeNodes;

	public TMP_InputField searchField;

	public static Color searchSelectionNodeColor;

	public static Color searchSelectionPartColor;

	public Button clearButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDTechTreeSearchBar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchField_OnValueChange(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchField_OnClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSearchRoutine_003Ed__16))]
	protected virtual IEnumerator SearchRoutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchForNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeselectSearchResults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPartIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MatchType TagMatchType(ref string tag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool TermMatchesTag(string term, string tag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string[] SearchTagSplit(string terms)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool PartMatchesSearch(AvailablePart part, string[] terms)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearSearchBox()
	{
		throw null;
	}
}
