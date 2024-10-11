using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MENodeCategorizer : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CSearchRoutine_003Ed__63 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MENodeCategorizer _003C_003E4__this;

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
		public _003CSearchRoutine_003Ed__63(int _003C_003E1__state)
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

	public Transform nodeCategoryButtonTransformParent;

	public Transform displayedNodeIconTransformParent;

	public MENodeCategoryButton nodeCategoryButtonPrefab;

	public MEGUINodeIcon NodeIconPrefab;

	public TMP_InputField searchField;

	public PointerClickHandler searchFieldClickHandler;

	public float searchKeystrokeDelay;

	[SerializeField]
	private TextMeshProUGUI NotFoundText;

	[SerializeField]
	private ToggleGroup toggleGroup;

	private List<MENodeCategoryButton> categoryButtons;

	private List<MEGUINodeIcon> displayedNodeIcons;

	private List<MEGUINodeIcon> storedNodeIcons;

	private MEGUINodeIconGroup _toolboxNodeIconGroup;

	private MEGUINodeIconGroup _canvasNodeIconGroup;

	private Transform nodeCategorizerButtonRepository;

	private string lastDisplayedCategory;

	[SerializeField]
	protected GameObject categoryIconLoaderPrefab;

	[NonSerialized]
	public IconLoader categoryIconLoader;

	[SerializeField]
	protected GameObject nodeIconLoaderPrefab;

	[NonSerialized]
	public IconLoader nodeIconLoader;

	public MEBasicNodeListFilterList<MEGUINodeIcon> ExcludeFilters;

	private string[] searchTerms;

	private bool showingSearchResults;

	private float searchTimer;

	private Coroutine searchRoutine;

	private Action onSearchRoutineFinish;

	public static MENodeCategorizer Instance
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

	public bool ShowingSearchResults
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodeCategorizer()
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
	public void ResetStoredIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearNodesInCategory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal MEGUINodeIcon AddCanvasNodeButton(MEBasicNode basicNode, MEGUINode guiNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemoveCanvasNodeButton(MEGUINodeIcon nodeIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshCurrentDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisplayNodesInCategory(string newCategory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeselectLastCategoryButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Icon GetCategoryImage(string category)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayStoredIcon(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayStoredIcon(MEGUINodeIcon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighLightDisplayedNodeIcon(bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HightLightNodeIcon(MEGUINodeIcon nodeIcon, bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighlightNodes(bool highlight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HighlightNodes(Func<MEGUINodeIcon, bool> criteria, bool highlight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HightLightCategoryButton(bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HightLightCategoryButton(string categoryName, bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ContainsCategoryConfig(string categoryName, ConfigNode[] nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HightLightCategoryButton(MENodeCategoryButton categoryButton, bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUINodeIcon[] GetNodes(Func<MEGUINodeIcon, bool> criteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDrag(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDrag(Func<MEGUINodeIcon, bool> criteria, bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDisplayedNodeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSearchField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusSearchField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchField_OnEndEdit(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchField_OnValueChange(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SearchField_OnValueChange(string s, Action ballback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchField_OnClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSearchRoutine_003Ed__63))]
	private IEnumerator SearchRoutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void HighLightSelectedSearchResults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateSearchToolboxGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveSearchToolboxGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool NodeMatchesSearch(MEGUINodeIcon node)
	{
		throw null;
	}
}
