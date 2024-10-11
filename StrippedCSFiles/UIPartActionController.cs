using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class UIPartActionController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMouseClickCoroutine_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIPartActionController _003C_003E4__this;

		public Camera cam;

		public bool allowMultiple;

		private float _003Ctime_003E5__2;

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
		public _003CMouseClickCoroutine_003Ed__39(int _003C_003E1__state)
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

	public UIPartActionWindow windowPrefab;

	public List<UIPartActionFieldItem> fieldPrefabs;

	private List<Type> fieldControlTypes;

	public UIPartActionGroup groupPrefab;

	public UIPartActionButton eventItemPrefab;

	public UIPartActionResource resourceItemPrefab;

	public UIPartActionResourceTransfer resourceTransferItemPrefab;

	public UIPartActionResourceEditor resourceItemEditorPrefab;

	public UIPartActionResourcePriority resourcePriorityPrefab;

	public UIPartActionFuelFlowOverlay fuelFlowOverlayPrefab;

	public UIPartActionAeroDisplay debugAeroItemPrefab;

	public UIPartActionThermalDisplay debugThermalItemPrefab;

	public UIPartActionRoboticJointDisplay debugRoboticJointItemPrefab;

	public float zMin;

	private bool guiActive;

	public List<UIPartActionWindow> windows;

	public List<UIPartActionWindow> hiddenWindows;

	public List<UIPartActionWindow> hiddenResourceWindows;

	private bool allowControl;

	public UIPartActionControllerInventory partInventory;

	private bool showWindows;

	private bool isClicking;

	[SerializeField]
	private List<int> _resourcesShown;

	private bool resourcesShownDirty;

	private List<UIPartActionResourceTransfer> transfers;

	public static UIPartActionController Instance
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

	public List<int> resourcesShown
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionController()
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
	private void OnCameraChange(CameraManager.CameraMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupItemControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFieldItem GetFieldControl(Type uiControlType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionItem GetControl(Type uiControlType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateActiveWindows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceWindows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TrySelect(Camera cam, bool allowMultiple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CMouseClickCoroutine_003Ed__39))]
	private IEnumerator MouseClickCoroutine(Camera cam, bool allowMultiple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HandleMouseClick(Camera cam, bool allowMultiple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnPartActionWindow(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnPartActionWindow(Part part, bool overrideSymmetry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectPart(Part part, bool allowMultiple, bool overrideSymmetry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetSelectionModifier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deselect(bool resourcesToo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PartWindowIsFromAnInventory(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIPartActionWindow CreatePartUI(Part part, UIPartActionWindow.DisplayType type, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ItemListContains(Part part, bool includeSymmetryCounterparts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ItemListContainsCounterparts(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionWindow GetItem(Part part, bool includeSymmetryCounterparts = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionWindow ItemListGet(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIPartActionWindow HiddenItemListGet(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIPartActionWindow HiddenResourceItemListGet(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnShowResource(ResourceItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHideResource(ResourceItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShowTransfers(PartResource res)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupResourceTransfer(UIPartActionResourceTransfer trans)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool InventoryAndCargoPartExist()
	{
		throw null;
	}
}
