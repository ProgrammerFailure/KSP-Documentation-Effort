using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEActionPane : MonoBehaviour
{
	public static MEActionPane fetch;

	public MEGUIPanel SAPPanel;

	[SerializeField]
	private ScrollRect sapScrollRect;

	private bool sapNeedsRefocus;

	private Bounds sapRectTransformBounds;

	private float sapFocusCalc;

	public MEGUIPanel GAPPanel;

	[SerializeField]
	private LayoutElement gapLayoutElement;

	[SerializeField]
	private int minGapPanelHeight;

	private MEGUIParameter lockedSAPParameter;

	private MENode currentLockedNode;

	public MEGUIFooterAdditionalButton FooterAdditionalButtonPrefab;

	public MEGUIParameterGroup ParameterGroupPrefab;

	public Toggle gapLockToggle;

	[SerializeField]
	private int parameterGroupOffset;

	[SerializeField]
	private GameObject sapFixedHeader;

	[SerializeField]
	private RectTransform sapHeaderParamHolder;

	private Image sapScrollRectBackgroundImage;

	private List<string> sapFrozenParamIDs;

	[SerializeField]
	protected GameObject gapIconLoaderPrefab;

	[NonSerialized]
	public IconLoader gapIconLoader;

	protected Dictionary<MonoBehaviour, Dictionary<string, MEGUIParameter>> parameterCache;

	protected Dictionary<string, MEGUIParameter> currentParameterCache;

	protected Transform cacheTmpLocation;

	private List<TMP_InputField> inputTabStops;

	private int currentInputField;

	private string currentInputFieldText;

	public ActionPaneDisplay CurrentGapDisplay
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public MEGUIParameter SelectedSAPParameter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public bool IsGAPLocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEActionPane()
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
	internal void OnParameterClick(MEGUIParameter paramClicked)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SAPRefreshNodeParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SAPDisplayNodeParameters(MEGUINode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SAPDisplayConnectorParameters(MEGUIConnector connector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SAPDisplayMultipleSelectedItemsMessage(int nodesSelected, string itemName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayNodeSettingsSection(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayNodeModuleSection(IMENodeDisplay module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterGroup DisplayModuleHeader(string name, string displayName, BaseAPFieldList fields, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayModuleFooter(string name, BaseAPFieldList fields)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MEGUIParameter DisplayParameter(BaseAPField field, Transform parent, bool isSelectable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterGroup AddParameterBlock(string headerName, string displayName, BaseAPFieldList fields, Transform parent, List<string> invalidGroups = null, string currentGroup = "", bool parametersSelectable = true, int offset = 0, int depth = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameterGroup(string group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetParameterFromFieldID(string fieldID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSapScrollbarValueChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GAPInitialize<T>() where T : ActionPaneDisplay
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clean()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnGAPLockValueChange(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollBar(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSAPScroll(PointerEventData pointerData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScrollPanel(float height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetMaxPanelWidth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SavePreferedGapSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitializeGapMinHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckDeletedNodeFromGap(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CurrentLockdeNode(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLockInteractivity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetInputFieldsList(Dictionary<string, MEGUIParameter> currentParam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_InputField GetNextInputTabStop(bool direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputfieldStringCatcher(string text)
	{
		throw null;
	}
}
