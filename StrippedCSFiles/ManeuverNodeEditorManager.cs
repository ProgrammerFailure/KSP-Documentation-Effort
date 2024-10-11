using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManeuverNodeEditorManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	internal class UsageStats
	{
		public int leftTabChange;

		public int rightTabChange;

		public int nodeSelector;

		public int warpTo;

		public int deleteNode;

		public int vectorHandle;

		public int vectorText;

		public int utHandle;

		public int utText;

		public int precisionSlider;

		public int orbitSelection;

		internal bool UsageFound
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UsageStats()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CManNodeCreateCooldown_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ManeuverNodeEditorManager _003C_003E4__this;

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
		public _003CManNodeCreateCooldown_003Ed__54(int _003C_003E1__state)
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

	public static ManeuverNodeEditorManager Instance;

	public List<ManeuverNodeEditorTab> maneuverNodeEditorTabs;

	public Button deleteNodeButton;

	public Button nextNodeButton;

	public Button prevNodeButton;

	public Button warpToNextNodeButton;

	public TextMeshProUGUI nodeName;

	private ManeuverNode _selectedManeuverNode;

	private int _selectedManNodeIndex;

	private Vector3d newDeltaV;

	private bool _mouseWithinTool;

	private bool ready;

	[SerializeField]
	private ManeuverNodeEditorTabButton tabButtonPrefab;

	[SerializeField]
	private RectTransform leftTabButtonsParent;

	[SerializeField]
	private RectTransform leftTabsParent;

	[SerializeField]
	private RectTransform rightTabButtonsParent;

	[SerializeField]
	private RectTransform rightTabsParent;

	private List<ManeuverNodeEditorTab> leftTabs;

	private List<ManeuverNodeEditorTab> rightTabs;

	private List<ManeuverNodeEditorTabButton> leftTabButtons;

	private List<ManeuverNodeEditorTabButton> rightTabButtons;

	private bool justSelected;

	private WaitForSeconds selectionCooldown;

	private ManeuverNode previousManNode;

	private bool isActive;

	private double tempWarpToUT;

	private static string cacheAutoLOC_7001217;

	private static string cacheAutoLOC_198614;

	internal UsageStats usage;

	public ManeuverNode SelectedManeuverNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int SelectedManNodeIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool MouseWithinTool
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsReady
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ManeuverNodeEditorTab> LeftTabs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ManeuverNodeEditorTab> RightTabs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool JustSelectedNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorManager()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTabButtons(ref List<ManeuverNodeEditorTabButton> tabButtonList, ref List<ManeuverNodeEditorTab> tabList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void addTab(ManeuverNodeEditorTab newItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabButton GetTabToggle(int index, ManeuverNodeEditorTabPosition type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ModifyBurnVector(NavBallVector axis, double amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetManeuverNodeInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssignSelectedManeuverNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnManNodeDeselected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnManNodeSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ChangeTooltipText(TooltipController_Text toolTip, string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnTargetChanged(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CManNodeCreateCooldown_003Ed__54))]
	private IEnumerator ManNodeCreateCooldown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDeltaVInNodeComponents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void setupAllTabs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTabButtonPressed(bool pressedToggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCurrentTab(int selected, ManeuverNodeEditorTabPosition type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateButtonAvailability()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUIModeFinishedChanging(FlightUIMode data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteManNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NextManNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrevManNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TriggerManNodeChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WarpToNextManNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMouseOverGizmo(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPauseButtonState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapEntered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapExited()
	{
		throw null;
	}
}
