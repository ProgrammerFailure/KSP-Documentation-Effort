using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class BasePartCategorizer : MonoBehaviour
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
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public BasePartCategorizer _003C_003E4__this;

		public string[] searchTerms;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public _003C_003Ec__DisplayClass31_0()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool _003CSearchRoutine_003Eb__0(AvailablePart p)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSearchRoutine_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BasePartCategorizer _003C_003E4__this;

		private _003C_003Ec__DisplayClass31_0 _003C_003E8__1;

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
		public _003CSearchRoutine_003Ed__31(int _003C_003E1__state)
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

	public UIList scrollListSub;

	[SerializeField]
	protected GameObject iconLoaderPrefab;

	[NonSerialized]
	public IconLoader iconLoader;

	public TMP_InputField searchField;

	public float searchKeystrokeDelay;

	protected Image searchFieldBackground;

	protected PointerClickHandler searchFieldClickHandler;

	[NonSerialized]
	public Color colorFilterFunction;

	[NonSerialized]
	public Color colorIcons;

	protected float searchTimer;

	protected Coroutine searchRoutine;

	protected EditorPartListFilter<AvailablePart> filterPods;

	protected EditorPartListFilter<AvailablePart> filterEngine;

	protected EditorPartListFilter<AvailablePart> filterFuelTank;

	protected EditorPartListFilter<AvailablePart> filterControl;

	protected EditorPartListFilter<AvailablePart> filterStructural;

	protected EditorPartListFilter<AvailablePart> filterCoupling;

	protected EditorPartListFilter<AvailablePart> filterPayload;

	protected EditorPartListFilter<AvailablePart> filterAero;

	protected EditorPartListFilter<AvailablePart> filterGround;

	protected EditorPartListFilter<AvailablePart> filterThermal;

	protected EditorPartListFilter<AvailablePart> filterElectrical;

	protected EditorPartListFilter<AvailablePart> filterCommunication;

	protected EditorPartListFilter<AvailablePart> filterScience;

	protected EditorPartListFilter<AvailablePart> filterCargo;

	protected EditorPartListFilter<AvailablePart> filterRobotics;

	protected EditorPartListFilter<AvailablePart> filterUtility;

	protected static string[] size0Tags;

	protected static string[] size1Tags;

	protected static string[] size1p5Tags;

	protected static string[] size2Tags;

	protected static string[] size3Tags;

	protected static string[] size4Tags;

	protected static string[] srfTags;

	protected static string[] xfeedTags;

	protected static string[] mannedTags;

	protected static string[] unmannedTags;

	protected static string[] radialTag;

	protected static string[] cargoTag;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BasePartCategorizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BasePartCategorizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchField_OnEndEdit(string s)
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
	[IteratorStateMachine(typeof(_003CSearchRoutine_003Ed__31))]
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
	protected virtual void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
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
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadAutoTags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GeneratePartAutoTags(AvailablePart p)
	{
		throw null;
	}
}
