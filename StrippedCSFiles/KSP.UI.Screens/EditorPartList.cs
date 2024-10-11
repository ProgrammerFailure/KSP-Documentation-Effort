using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorPartList : MonoBehaviour
{
	public enum State
	{
		PartsList,
		SubassemblyList,
		CustomPartList,
		PartSearch,
		Nothing,
		VariantsList
	}

	[CompilerGenerated]
	private sealed class _003CAutoScrollBarFix_routine_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorPartList _003C_003E4__this;

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
		public _003CAutoScrollBarFix_routine_003Ed__42(int _003C_003E1__state)
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
	private sealed class _003CScrollBarFix_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string key;

		public EditorPartList _003C_003E4__this;

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
		public _003CScrollBarFix_003Ed__44(int _003C_003E1__state)
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

	public EditorPartIcon partPrefab;

	public RectTransform partGrid;

	public EditorSubassemblyItem subassemblyPrefab;

	public RectTransform subassemblyGrid;

	public EditorVariantItem variantPrefab;

	public RectTransform variantGrid;

	public UIListSorter partListSorter;

	public UIScrollRectState scrollRectState;

	public ScrollRect partListScrollRect;

	public float iconOverSpin;

	public float iconOverScale;

	public float iconSize;

	public Button customPartButtonTransform;

	public Button subassemblyButtonTransform;

	public EditorPartListFilter<AvailablePart> AmountAvailableFilter;

	public EditorPartListFilter<AvailablePart> SearchFilterParts;

	public EditorPartListFilterList<AvailablePart> GreyoutFilters;

	public EditorPartListFilterList<AvailablePart> ExcludeFilters;

	public EditorPartListFilterList<AvailablePart> CategorizerFilters;

	public EditorPartListFilterList<ShipTemplate> ExcludeFiltersSubassembly;

	public bool allowTabChange;

	private IComparer<AvailablePart> currentPartSorting;

	private IComparer<ShipTemplate> currentSubassemblySorting;

	private List<AvailablePart> categoryList;

	private List<EditorPartIcon> icons;

	private State state;

	private GameObject partIconStorage;

	private Dictionary<string, float> scrollbarPositions;

	private string lastFilterID;

	private Dictionary<string, EditorPartIcon> iconCache;

	private List<EditorSubassemblyItem> saItems;

	private List<ShipTemplate> saList;

	private List<EditorVariantItem> vItems;

	public static EditorPartList Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorPartList()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartListUpdate(Vector2 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortingCallback(int button, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RememberScrollbarPosition(State currentState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateScrollbarPositionDictionary(string key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CAutoScrollBarFix_routine_003Ed__42))]
	private IEnumerator AutoScrollBarFix_routine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AutoScrollBarFix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CScrollBarFix_003Ed__44))]
	private IEnumerator ScrollBarFix(string key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh(State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearAllItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshSearchList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshCustomPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartIcons(bool customCategory = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartIcon(EditorPartIcon newIcon, AvailablePart availablePart, bool customCategory = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Refresh Subs")]
	public void RefreshSubassemblies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshSubassemblyList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Refresh Variants")]
	public void RefreshVariants()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshVariantsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TapIcon(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TapIcon(ShipTemplate st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TapBackground()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RevealPart(AvailablePart partToFind, bool revealToolip)
	{
		throw null;
	}
}
