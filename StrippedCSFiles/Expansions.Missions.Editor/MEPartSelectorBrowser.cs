using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEPartSelectorBrowser : MonoBehaviour
{
	public delegate void CancelledCallback();

	public delegate void SelectionCallback(List<string> selectedParts);

	public delegate void PartSelectionCallback(AvailablePart part, bool state);

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MEPartSelectorBrowser _003C_003E4__this;

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
		public _003CStart_003Ed__32(int _003C_003E1__state)
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
	private TextMeshProUGUI headerTitle;

	public MEPartSelectorEntry mePrefab;

	protected string title;

	public UIListSorter partListSorter;

	public MEPartCategorizer PartCategorizer;

	public ScrollRect partListScrollRect;

	public float iconSize;

	public float iconOverScale;

	public float iconOverSpin;

	public Button selectButton;

	public Button cancelButton;

	public CancelledCallback OnBrowseCancelled;

	public SelectionCallback OnBrowseSelectedParts;

	public PartSelectionCallback OnPartSelectedChange;

	[SerializeField]
	private RectTransform listContainer;

	protected List<MEPartSelectorEntry> partsList;

	protected Dictionary<string, List<string>> excludedParts;

	private List<string> _selectedParts;

	private Color _selectionColor;

	private Dictionary<string, MEPartSelectorEntry> iconCache;

	protected EditorPartListFilterList<AvailablePart> partsFilter;

	protected EditorPartListFilter<AvailablePart> searchFilter;

	private IComparer<AvailablePart> currentPartSorting;

	private Transform partIconStorage;

	public List<string> SelectedParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Color SelectionColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartSelectorBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__32))]
	public IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartSelectorBrowser Spawn(List<string> partList, Dictionary<string, List<string>> excludedParts, Color selectionColor, SelectionCallback onSelect, CancelledCallback onCancel, RectTransform parent = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPartSelectorBrowser Spawn(MEGUIParameterPartPicker parameter, PartSelectionCallback onPartSelect, RectTransform parent = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(MEGUIParameterPartPicker parameter, PartSelectionCallback onPartSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onButtonSelect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisablePart(MEPartSelectorEntry entry, bool status, string excludeReason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddPartIcon(MEPartSelectorEntry entry, RectTransform listParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartListUpdate(Vector2 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartSelectionStatus(AvailablePart part, bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartSelectionChange(AvailablePart part, bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnFilterSelectionChange(EditorPartListFilter<AvailablePart> filter, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSearchChange(EditorPartListFilter<AvailablePart> filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortingCallback(int button, bool asc)
	{
		throw null;
	}
}
